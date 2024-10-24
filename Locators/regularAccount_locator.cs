using System.Collections.Generic;

namespace Locators {
    public static class RegularAccountLocator {
        public static readonly Dictionary<string, string> LoginReg = new Dictionary<string, string>
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
            { "ms_Cad_Suc", "//a[@href='/#'][contains(.,'Cadastro realizado com sucesso')]" },
            { "login", "//h1[@class='font-robot'][contains(.,'Login')]" }
        };

        public static readonly Dictionary<string, string> HomeReg = new Dictionary<string, string>
        {
            { "inputSear", "//input[contains(@data-testid,'pesquisar')]" },
            { "btSear", "//button[contains(@data-testid,'botaoPesquisar')]" },
            { "msNoProdu", "//p[contains(.,'Nenhum produto foi encontrado')]" },
            { "btLogOut", "//button[contains(@data-testid,'logout')]" },
            { "detail", "//a[@class='card-link'][contains(.,'Detalhes')]" },
            { "msDetail", "//h1[contains(.,'Detalhes do produto')]" }
        };
    }
}
