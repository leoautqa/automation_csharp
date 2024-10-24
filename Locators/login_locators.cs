using System.Collections.Generic;

namespace Locators {
    public static class LoginLocator {
        public static readonly Dictionary<string, string> login = new Dictionary<string, string>
        {
            { "h1Login", "//h1[@class='font-robot'][contains(.,'Login')]" },
            { "inputEmail", "//input[contains(@data-testid,'email')]" },
            { "inputPassword", "//input[contains(@data-testid,'senha')]" },
            { "butEnter", "//button[contains(@data-testid,'entrar')]" },
            { "mandatoryEmail", "//span[contains(.,'Email é obrigatório')]" },
            { "mandatoryPassword", "//span[contains(.,'Password é obrigatório')]" },
            { "messageInvalid", "//span[contains(.,'Email e/ou senha inválidos')]" },
            { "register", "//a[contains(@data-testid,'cadastrar')]" },
            { "mandatoryName", "//div[@class='alert alert-secondary alert-dismissible'][contains(.,'×Nome é obrigatório')]" },
            { "btRegistration", "//button[contains(@data-testid,'cadastrar')]" },
            { "userName", "//input[contains(@data-testid,'nome')]" },
            { "userEmail", "//input[contains(@data-testid,'email')]" },
            { "userPassword", "//input[contains(@data-testid,'password')]" },
            { "successfulMessage", "//a[@href='/#'][contains(.,'Cadastro realizado com sucesso')]" },
            { "logout", "//button[contains(@data-testid,'logout')]" },
            { "MessageAlreadyExists", "//span[contains(.,'Este email já está sendo usado')]" },
            { "checkbox", "//input[contains(@data-testid,'checkbox')]" }
        };
    }
}
