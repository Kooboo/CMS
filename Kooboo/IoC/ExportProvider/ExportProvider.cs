using System;
using System.Collections.Generic;
using System.Collections;
using System.Reflection;
using System.ComponentModel.Composition;
using System.Linq;


namespace Kooboo.IoC
{
    public class ExportProvider:IExportProvider
    {
        public ExportProvider()
        {
        }

        public ExportProvider(ITypeEnumerator typeEnumerator)
        {
            if (typeEnumerator != null)
            {
                TypeEnumerator = typeEnumerator;
            }
        }

        public IEnumerable<Contract> FindExports(Type contractType)
        {
            if (Contracts.ContainsKey(contractType))
            {
                return Contracts[contractType];
            }
            else
            {
                return Enumerable.Empty<Contract>();
            }
        }

        public void Export(Type exportType, Type contractType, string contractName, int index)
        {
            AddContract(Contracts, exportType, contractType, contractName, index);
        }

        ITypeEnumerator TypeEnumerator = new TypeEnumerator();

        Dictionary<Type, List<Contract>> _Contracts;
        Dictionary<Type, List<Contract>> Contracts
        {
            get
            {
                if (_Contracts == null)            
                {
                    lock (_LockObject)
                    {
                        _Contracts = ExportAll();
                    }                
                }

                return _Contracts;
            }
            set
            {
                _Contracts = value;
            }
        }
        volatile object _LockObject = new object();

        Dictionary<Type, List<Contract>> ExportAll()
        {

            Dictionary<Type, List<Contract>> list = new Dictionary<Type, List<Contract>>();
            var types = TypeEnumerator.GetTypes();

            foreach (var type in types)
            {
                if (type != null)
                {

                    var orderedExports = type.GetCustomAttributes(typeof(OrderedExportAttribute), false);
                    foreach (OrderedExportAttribute export in orderedExports)
                    {
                        AddContract(list, type, export.ContractType, export.ContractName, export.Index);
                    }


                    var exports = type.GetCustomAttributes(typeof(ExportAttribute), false);
                    foreach (ExportAttribute export in exports)
                    {
                        AddContract(list, type, export.ContractType,export.ContractName);
                    }

                    
               
                }
            }

            return list;
           
        }

        void AddContract(Dictionary<Type, List<Contract>> list,Type exportType, Type contractType,string contractName,int exportIndex=int.MaxValue)
        {
            //TODO: should check the converter..
            //if (to.IsAssignableFrom(bind) && bind.GetConstructor(Type.EmptyTypes) != null)
            //{
            if (contractType == null)
            {
                AddContract(list, exportType, exportType, contractName,exportIndex);
            }
            else
            {
                if (list.ContainsKey(contractType) == false)
                {
                    list.Add(contractType, new List<Contract>());
                }

                if (list[contractType].Any(i=>i.ExportType ==exportType) == false)
                {
                    list[contractType].Add(new Contract{ ExportType = exportType, Name= contractName,Index = exportIndex});
                }
            }
            //}
            //else
            //{

            //}
        }
    }
}
