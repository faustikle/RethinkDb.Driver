












//AUTOGENERATED, DO NOTMODIFY.
//Do not edit this file directly.

#pragma warning disable 1591
// ReSharper disable CheckNamespace

using System;
using RethinkDb.Driver.Ast;
using RethinkDb.Driver.Model;
using RethinkDb.Driver.Proto;
using System.Collections.Generic;


namespace RethinkDb.Driver.Ast {

    public class Funcall : ReqlExpr {

    
        private bool swappedArgs = false;

    
    
        public Funcall (object arg) : this(new Arguments(arg), null) {
        }
        public Funcall (Arguments args) : this(args, null) {
        }
        public Funcall (Arguments args, OptArgs optargs)
             : this(TermType.FUNCALL, args, optargs) {
        }

    protected Funcall (TermType termType, Arguments args, OptArgs optargs) : base(termType, args, optargs)
    {
    }


    



    


    








       




    
    
        /*
          This object should be constructed with arguments first, and the
          function itself as the last parameter.  This makes it easier for
          the places where this object is constructed.  The actual wire
          format is function first, arguments last, so we flip them around
          when building the AST.
        */
        protected internal override object Build() {
            
            if( !swappedArgs )
            {
                var lastIdx = this.Args.Count - 1;
                var func = this.Args[lastIdx];
                this.Args.RemoveAt(lastIdx);
                this.Args.Insert(0, func);
                swappedArgs = true;
            }


            return base.Build();
        }



    
    }
}
