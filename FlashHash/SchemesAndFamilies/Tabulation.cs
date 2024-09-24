using Perfolizer.Mathematics.Randomization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace FlashHash.SchemesAndFamilies
{
    public class TabulationFamily : IHashFunctionFamily<TabulationScheme>
    {
        static Random _random = new Random();

        public TabulationScheme GetScheme(ulong size, ulong offset)
        {
            return new TabulationScheme(GetTable(), size, offset);
        }

        public TabulationFamily() { }

        public static ulong[][] GetTable()
        {
            ulong[][] table = new ulong[8][];
            for (int i = 0; i < 8; i++)
            {
                table[i] = new ulong[256];

                var t = table[i];
                for (int j = 0; j < 256; j++)
                {
                    t[j] =
                        ((((ulong)RandomNumberGenerator.GetInt32(int.MinValue, int.MaxValue)) << 32)) + ((ulong)RandomNumberGenerator.GetInt32(int.MinValue, int.MaxValue));
                }
            }
            return table;

        }

        public void SetRandomness(Random random)
        {
            _random = random;
        }

        IHashFunctionScheme IHashFunctionFamily.GetScheme(ulong size, ulong offset)
        {
            return GetScheme(size, offset);
        }
    }

    public readonly record struct TabulationScheme(ulong[][] Table, ulong Size, ulong Offset) : IHashFunctionScheme
    {
        //static string name = "Name: A";
        public Expression<HashingFunction> Create()
        {
            const int length = 8;

            var f = CompiledFunctions.Create<ulong, ulong>(out var input_);

            f.S.DeclareVariable<int>(out var i_, 0)

            //.DeclareVariable<ulong>(out var value_)

            .DeclareVariable<ulong[][]>(out var table_, Table)
            //.Print("H").
            //Print(input_.V.ToStringExpression())
            .Assign(f.Output, 0)
            .While(i_.V < length,
                new Scope().DeclareVariable<int>(out var index_, (input_.V % (1 << length)).Convert<int>())
                .Assign(input_, input_.V >> length)
                .DeclareVariable<ulong>(out var value_, table_.V.ToTable<ulong[]>()[i_.V].V.ToTable<ulong>()[index_.V].V)
                .Assign(f.Output, f.Output.V ^ value_.V)
                .Assign(i_, i_.V + 1))
            .Assign(f.Output, f.Output.V % Size + Offset)
            //.Print(f.Output.V.ToStringExpression())

            ;

            //f.S.Print(f.Output.V.ToStringExpression());
            //ToDo find why Output leaks
            //f.S.Print("->");
            //f.S.Print(input_.V.ToStringExpression());
            //for (var i = 0; i < Table.Length; i++)
            //{
            //	var e = Map.GetMap(Enumerable.Range(0, Table[i].Length).Select(x => (ulong)x).Zip(Table[i]));
            //	f.S.Assign(value_, f.S.Function(e, input_.V % (1 << length)))
            //		.Assign(input_, input_.V >> length)
            //		.Assign(f.Output, f.Output.V ^ value_.V);



            //}

            f.S.Assign(f.Output, f.Output.V % Size + Offset);
            ///f.S.Print(name);
            //f.S.Print(f.Output.V.ToStringExpression());
            //f.S.Print("<-");
            //name = name + "A";
            return f.Construct();
        }

        public bool Equals(IHashFunctionScheme? other)
        {
            throw new NotImplementedException();
        }
    }
}
