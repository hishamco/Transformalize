﻿#region license
// Transformalize
// Configurable Extract, Transform, and Load
// Copyright 2013-2020 Dale Newman
//  
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//   
//       http://www.apache.org/licenses/LICENSE-2.0
//   
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
#endregion

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Transformalize.Configuration;
using Transformalize.Contracts;
using Transformalize.Extensions;

namespace Transformalize.Providers.File {

    public class FileInspection : ICreateConfiguration {

        private readonly IConnectionContext _context;
        private readonly FileInfo _fileInfo;
        private readonly int _lines;

        public FileInspection(
            IConnectionContext context,
            FileInfo fileInfo,
            int lines = 100) {
            _context = context;
            _fileInfo = fileInfo;
            _lines = lines;
        }

        public string Create() {

            var identifier = Utility.Identifier(_fileInfo.Name.Replace(_fileInfo.Extension, string.Empty));
            var quoted = _fileInfo.Extension.ToLower() == ".csv";

            var lines = new FileLineReader(_fileInfo, _lines).Read().ToArray();
            var delimiters = _context.Connection.Delimiters.Any() ?
                _context.Connection.Delimiters :
                new List<Delimiter> {
                    new Delimiter {
                        Character = (_context.Connection.Delimiter.Length == 0 ? ',' : _context.Connection.Delimiter[0]),
                        Name = "Delimiter"
                    }
                };
            var delimiter = Utility.FindDelimiter(lines, delimiters, quoted);

            var values = lines.First()
                .SplitLine(delimiter, quoted)
                .Select(c => c.Trim('"'))
                .Select(c => c.Trim())
                .ToArray();

            // substitute blank headers with excel column names (useful when some of the column headers are blank)
            for (var i = 0; i < values.Length; i++) {
                if (values[i] == string.Empty) {
                    values[i] = Utility.GetExcelName(i);
                }
            }

            var hasColumnNames = ColumnNames.AreValid(_context, values);
            var fieldNames = hasColumnNames ? values : ColumnNames.Generate(values.Length).ToArray();

            var connection = new Connection {
                Name = "input",
                Provider = "file",
                File = _fileInfo.FullName,
                Delimiter = delimiter == default(char) ? "," : delimiter.ToString(),
                Start = hasColumnNames ? 2 : 1,
                Types = _context.Connection.Types
            };

            var process = new Process {
                Name = "FileInspector",
                Pipeline = "parallel.linq",
                ReadOnly = true,
                Connections = new List<Connection> { connection }
            };

            process.Entities.Add(new Entity {
                Name = identifier,
                PrependProcessNameToOutputName = false,
                Sample = Convert.ToInt32(_context.Connection.Sample)
            });

            foreach (var name in fieldNames) {
                process.Entities[0].Fields.Add(new Field {
                    Name = name,
                    Alias = Constants.InvalidFieldNames.Contains(name) ? identifier + name : name,
                    Length = "max"
                });
            }

            process.Load();

            foreach (var warning in process.Warnings()) {
                _context.Warn(warning);
            }

            foreach (var error in process.Errors()) {
                _context.Error(error);
            }

            return process.Serialize();

        }

    }
}
