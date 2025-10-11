using ContractSystem.Core.DTO;
using ContractSystem.Core.Models.In;
using ContractSystem.Core.Models.Out;
using Mapster;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContractSystem.Core
{
    public class MapsterConfig : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<UserDTO, UserOut>();
            config.NewConfig<UserIn, UserDTO>();

            config.NewConfig<DocumentDTO, DocumentOut>();
            config.NewConfig<DocumentIn, DocumentDTO>();

            config.NewConfig<ApprovalDTO, ApprovalOut>();
            config.NewConfig<ApprovalIn, ApprovalDTO>();
        }
    }
}
