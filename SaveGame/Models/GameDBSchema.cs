using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaveGame.Models
{
    internal class GameDBSchema
    {
        public long? Id { get; set; }
        public PlayStates PlayState { get; set; }
    }
}
