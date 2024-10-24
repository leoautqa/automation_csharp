using System.Collections.Generic;

namespace Locators {
    public static class AdministratorAccountLocator {
        public static readonly Dictionary<string, string> LoginAdm = new Dictionary<string, string>
        {
            { "inputEmail", "//input[contains(@data-testid,'email')]" },
            { "inputPassword", "//input[contains(@data-testid,'senha')]" },
            { "butEnter", "//button[contains(@data-testid,'entrar')]" },
            { "messageInvalid", "//span[contains(.,'Email e/ou senha inválidos')]" },
            { "userName", "//input[contains(@data-testid,'nome')]" },
            { "userEmail", "//input[contains(@data-testid,'email')]" },
            { "userPassword", "//input[contains(@data-testid,'password')]" },
            { "checkbox", "//input[contains(@data-testid,'checkbox')]" },
            { "btSign_Up", "//a[contains(@data-testid,'cadastrar')]" },
            { "btRegister", "//button[contains(@data-testid,'cadastrar')]" },
            { "ms_Cad_Suc", "//a[@href='/#'][contains(.,'Cadastro realizado com sucesso')]" }
        };

        public static readonly Dictionary<string, string> HomeAdm = new Dictionary<string, string>
        {
            { "hCadUse", "//h5[@class='card-title'][contains(.,'Cadastrar Usuários')]" },
            { "hLisUse", "//h5[@class='card-title'][contains(.,'Listar Usuários')]" },
            { "hCadProd", "//h5[@class='card-title'][contains(.,'Cadastrar Produtos')]" },
            { "hLisProd", "//h5[@class='card-title'][contains(.,'Listar Produtos')]" },
            { "hRel", "//h5[@class='card-title'][contains(.,'Relatórios')]" }
        };

        public static readonly Dictionary<string, string> CadUsu = new Dictionary<string, string>
        {
            { "btCad", "//button[contains(@data-testid,'cadastrarUsuario')]" },
            { "msgNameMan", "//span[contains(.,'Nome é obrigatório')]" },
            { "msgEmailMan", "//span[contains(.,'Email é obrigatório')]" },
            { "msgPassMan", "//span[contains(.,'Password é obrigatório')]" },
            { "msAlrExi", "//span[contains(.,'Este email já está sendo usado')]" },
            { "inpName", "//input[contains(@data-testid,'nome')]" },
            { "inpEmail", "//input[contains(@data-testid,'email')]" },
            { "inpPass", "//input[contains(@data-testid,'password')]" },
            { "cheBoxAdm", "//input[contains(@data-testid,'checkbox')]" }
        };

        public static readonly Dictionary<string, string> LisUsu = new Dictionary<string, string>
        {
            { "hUmLisUsu", "//h1[contains(.,'Lista dos usuários')]" }
        };

        public static readonly Dictionary<string, string> CadProd = new Dictionary<string, string>
        {
            { "btCad", "//button[contains(@data-testid,'cadastarProdutos')]" },
            { "msNoName", "//span[contains(.,'Nome é obrigatório')]" },
            { "msNoPrice", "//span[contains(.,'Preco é obrigatório')]" },
            { "msNoDesc", "//span[contains(.,'Descricao é obrigatório')]" },
            { "msNoQuant", "//span[contains(.,'Descricao é obrigatório')]" },
            { "nameProd", "//input[contains(@data-testid,'nome')]" },
            { "priceProd", "//input[contains(@data-testid,'preco')]" },
            { "descProd", "//textarea[contains(@data-testid,'descricao')]" },
            { "quantProd", "//input[contains(@data-testid,'quantity')]" },
            { "msPrice", "//span[contains(.,'Preco deve ser um número positivo')]" },
            { "msQuant", "//span[contains(.,'Quantidade deve ser maior ou igual a 0')]" },
            { "choFil", "//input[contains(@data-testid,'imagem')]" }
        };

        public static readonly Dictionary<string, string> LisProd = new Dictionary<string, string>
        {
            { "titLis", "//h1[contains(.,'Lista dos Produtos')]" },
            { "msProdExis", "//span[contains(.,'Já existe produto com esse nome')]" }
        };
    }
}
