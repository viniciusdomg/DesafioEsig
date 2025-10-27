# Desafio Técnico ESIG - Pessoa Desenvolvedora .NET

## Descrição

Aplicação ASP.NET Web Forms desenvolvida como parte do processo seletivo para a vaga de Estágio Pessoa Desenvolvedora .NET na ESIG Group. O objetivo principal é demonstrar habilidades em desenvolvimento web com .NET, C# e integração com banco de dados Oracle, através de uma aplicação que calcula, exibe e gerencia salários de pessoas.

## Tecnologias Utilizadas

* **Backend:** C#
* **Framework Web:** ASP.NET Web Forms (.NET Framework 4.7.2 ou superior)
* **Banco de Dados:** Oracle (Preferencialmente 11g ou superior)
* **Acesso a Dados:** ADO.NET com `Oracle.ManagedDataAccess.Client` (via NuGet)
* **Frontend:** HTML, CSS, Bootstrap 5 (para estilização)
* **Ambiente:** Visual Studio 2017 ou superior

## Configuração do Ambiente

Siga os passos abaixo para configurar e executar o projeto localmente:

### Pré-requisitos

* **Visual Studio:** Versão 2017 ou superior, com a carga de trabalho "Desenvolvimento ASP.NET e Web" instalada.
* **Oracle Database:** Versão 11g ou superior instalada e acessível.
* **Oracle Client/Driver:** Não é necessário instalar o Oracle Client completo. O projeto utiliza o `Oracle.ManagedDataAccess.Client` via NuGet, que será restaurado automaticamente.

### Banco de Dados Oracle

1.  **Crie um Usuário/Schema:** Crie um usuário no Oracle para a aplicação. Exemplo:
    ```sql
    CREATE USER ESIG IDENTIFIED BY 123;
    GRANT CONNECT, RESOURCE TO ESIG;
    GRANT CREATE SESSION TO ESIG;
    GRANT CREATE TABLE TO ESIG;
    GRANT CREATE SEQUENCE TO ESIG;
    GRANT CREATE TRIGGER TO ESIG;
    GRANT CREATE PROCEDURE TO ESIG;
    ALTER USER ESIG QUOTA UNLIMITED ON USERS;
    ```
    *(Conecte-se como SYS ou SYSTEM para executar esses comandos)*

2.  **Crie as Tabelas:** Conecte-se com o usuário `ESIG` e execute os seguintes scripts:

    * **Tabela `cargo`:**
        ```sql
        CREATE TABLE cargo (
            ID NUMBER PRIMARY KEY,
            Nome VARCHAR2(100) NOT NULL,
            Salario NUMBER(10, 2) NOT NULL
        );
        ```
    * **Tabela `pessoa`:**
        ```sql
        CREATE TABLE pessoa (
            ID NUMBER PRIMARY KEY,
            Nome VARCHAR2(255) NOT NULL,
            Cidade VARCHAR2(100),
            Email VARCHAR2(255),
            CEP VARCHAR2(10),
            Enderco VARCHAR2(255), -- Corrigido de ENDERCO
            Pais VARCHAR2(50),
            Usuario VARCHAR2(50),
            Telefone VARCHAR2(20),
            Data_Nascimento DATE,
            Cargo_ID NUMBER,
            CONSTRAINT fk_pessoa_cargo FOREIGN KEY (Cargo_ID) REFERENCES cargo (ID)
        );
        ```
    * **Tabela `pessoa_salario`:**
        ```sql
        CREATE TABLE pessoa_salario (
            pessoa_id NUMBER NOT NULL,
            pessoa_nome VARCHAR2(255),
            cargo_nome VARCHAR2(100),
            salario NUMBER(10, 2),
            CONSTRAINT fk_pessoa_salario_pessoa FOREIGN KEY (pessoa_id) REFERENCES pessoa (ID)
        );
        ```

3.  **Crie a Sequence (Geração de ID):**
    ```sql
    CREATE SEQUENCE pessoa_id_seq START WITH 1 INCREMENT BY 1;
    ```

4.  **Crie o Trigger (Geração de ID):**
    ```sql
    CREATE OR REPLACE TRIGGER trg_pessoa_before_insert
    BEFORE INSERT ON pessoa
    FOR EACH ROW
    BEGIN
      -- Pega o próximo valor da sequence e o atribui à coluna ID se ela for nula
      IF :new.ID IS NULL THEN
        :new.ID := pessoa_id_seq.NEXTVAL;
      END IF;
    END;
    /
    ```

5.  **Crie a Stored Procedure (Cálculo de Salários):**
    ```sql
    CREATE OR REPLACE PROCEDURE PRC_CALCULAR_SALARIOS AS
    BEGIN
        -- Limpa a tabela antes de inserir novos dados
        DELETE FROM pessoa_salario;

        -- Insere os dados combinados de pessoa e cargo
        INSERT INTO pessoa_salario (pessoa_id, pessoa_nome, cargo_nome, salario)
        SELECT
            p.ID,
            p.Nome,
            c.Nome,
            c.Salario
        FROM
            pessoa p
        INNER JOIN
            cargo c ON p.Cargo_ID = c.ID; -- Apenas pessoas com cargo válido

        COMMIT;
    END;
    /
    ```

6.  **Popule as Tabelas `cargo` e `pessoa` (Dados Iniciais):**
    * Você pode usar os arquivos CSV fornecidos. Utilize uma ferramenta como SQL Developer (Import Data) ou execute scripts `INSERT`.
    * **Dados da Tabela `pessoa`:** Os dados estão no arquivo `MATERIAL DE APOIO_ESTÁGIO PESSOA DESENVOLVEDORA .NET - pessoa.CSV`. Use a função de importação da sua ferramenta de banco de dados, configurando o delimitador como `;` e o formato de data apropriado (DD/MM/YYYY).

### Aplicação ASP.NET

1.  **Clone o Repositório:** Obtenha o código-fonte do projeto.
2.  **Abra no Visual Studio:** Abra o arquivo da solução (`.sln`).
3.  **Restaure os Pacotes NuGet:** Clique com o botão direito na Solução no Gerenciador de Soluções e escolha "Restaurar Pacotes do NuGet".
4.  **Configure a Connection String:**
    * Abra o arquivo `Web.config`.
    * Localize a seção `<connectionStrings>`.
    * Ajuste o valor do atributo `connectionString` na tag `<add name="ConnectionString" ... />` para corresponder aos dados do seu servidor Oracle local (usuário `ESIG`, senha `123`, e o `Data Source`, ex: `localhost:1521/XEPDB1`).
5.  **Defina a Página Inicial:**
    * No Gerenciador de Soluções, clique com o botão direito em `SalariosListagem.aspx`.
    * Selecione "Definir como Página Inicial".
6.  **Compile e Execute:**
    * Pressione `Ctrl+F5` ou clique no botão "Iniciar" (com IIS Express) para rodar a aplicação.

A aplicação deve abrir no seu navegador padrão na tela de Listagem de Salários.
