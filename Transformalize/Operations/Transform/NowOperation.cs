using System;
using System.Collections.Generic;
using Transformalize.Libs.Rhino.Etl;

namespace Transformalize.Operations.Transform {
    public class NowOperation : ShouldRunOperation {
        public NowOperation(string inKey, string outKey)
            : base(inKey, outKey) {
                Name = "Now(" + inKey + "=>" + outKey + ")";
        }

        public override IEnumerable<Row> Execute(IEnumerable<Row> rows) {
            foreach (var row in rows) {
                row[OutKey] = DateTime.Now;
                yield return row;
            }
        }
    }
}