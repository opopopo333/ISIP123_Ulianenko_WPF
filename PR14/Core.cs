using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PR14
{
    public class Core
    {
        // Контекст БД (EDMX)
        public static ULIPR14Entities Context = new ULIPR14Entities();

        // Текущий авторизованный пользователь
        public static Users CurrentUser;
    }
}
