-- Criar a base de dados "projeto" se ela não existir
CREATE DATABASE IF NOT EXISTS projeto;

-- Selecionar a base de dados "projeto"
USE projeto;

-- Criar a tabela "produto"
CREATE TABLE IF NOT EXISTS produto (
    id INT AUTO_INCREMENT PRIMARY KEY,
    nome VARCHAR(100) NOT NULL COMMENT 'Nome do produto',
    preco DECIMAL(10, 2) NOT NULL COMMENT 'Preço do produto',
    quantidade INT NOT NULL COMMENT 'Quantidade em estoque',
    unidade_por_compra INT NOT NULL COMMENT 'Unidade por compra',
    tipo ENUM('Acessorio', 'Eletronico', 'Eletrodomestico', 'Roupa', 'Casa') NOT NULL COMMENT 'Tipo de produto',
    marca VARCHAR(100) COMMENT 'Marca do produto',
    peso DECIMAL(10, 2) COMMENT 'Peso do produto (em kg)',
    altura DECIMAL(10, 2) COMMENT 'Altura do produto (em cm)',
    largura DECIMAL(10, 2) COMMENT 'Largura do produto (em cm)',
    descricao TEXT COMMENT 'Descrição do produto'
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

-- Inserir um produto com informações fictícias
INSERT INTO produto (nome, preco, quantidade, unidade_por_compra, tipo, marca, peso, altura, largura, descricao)
VALUES ('Notebook Exemplo', 2999.99, 50, 1, 'Eletronico', 'Marca Exemplo', 2.5, 2.0, 30.0, 'Um notebook de exemplo com especificações fictícias.');
