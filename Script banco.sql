
-- =========================
-- TABELA USUARIOS
-- =========================

CREATE TABLE Usuarios (
    IdUsuario INT PRIMARY KEY IDENTITY(1,1),
    Nome NVARCHAR(100) NOT NULL,
    Email NVARCHAR(150) UNIQUE NOT NULL,
    Senha NVARCHAR(255) NOT NULL,
    DataCriacao DATETIME DEFAULT GETDATE(),
    Ativo BIT DEFAULT 1,
    ADM BIT DEFAULT 1,
);

-- =========================
-- TABELA PRODUTOS
-- =========================

CREATE TABLE Produtos (
    IdProduto INT PRIMARY KEY IDENTITY(1,1),

    Nome NVARCHAR(150) NOT NULL,

    Descricao NVARCHAR(300),

    Categoria NVARCHAR(100) NOT NULL,

    Preco DECIMAL(10,2) NOT NULL,

    Custo DECIMAL(10,2),

    QuantidadeEstoque INT DEFAULT 0,

    Marca NVARCHAR(100),
    Tamanho NVARCHAR(20),

    Genero NVARCHAR(20),  -- Masculino / Feminino / Unissex

    Condicao NVARCHAR(20), -- Novo / Seminovo / Usado

    CodigoBarras NVARCHAR(50),

    DataCadastro DATETIME DEFAULT GETDATE(),

    Ativo BIT DEFAULT 1
);

-- =========================
-- TABELA VENDAS
-- =========================

CREATE TABLE Vendas (
    IdVenda INT PRIMARY KEY IDENTITY(1,1),

    ClienteDocumento NVARCHAR(20) NOT NULL,

    ClienteNome NVARCHAR(40) NOT NULL,

    DataVenda DATETIME DEFAULT GETDATE(),

    ValorTotal DECIMAL(10,2) NOT NULL,

    FormaPagamento NVARCHAR(50),

    StatusVenda NVARCHAR(20) DEFAULT 'Concluida'
);

-- =========================
-- TABELA PRODUTOVENDAS
-- =========================

CREATE TABLE ProdutoVendas (
    IdProdutoVenda INT PRIMARY KEY IDENTITY(1,1),

    VendaId INT NOT NULL,
    ProdutoId INT NOT NULL,

    Quantidade INT NOT NULL,

    PrecoUnitario DECIMAL(10,2) NOT NULL,

    Subtotal DECIMAL(10,2) NOT NULL,

    FOREIGN KEY (VendaId) REFERENCES Vendas(IdVenda),
    FOREIGN KEY (ProdutoId) REFERENCES Produtos(IdProduto)
);