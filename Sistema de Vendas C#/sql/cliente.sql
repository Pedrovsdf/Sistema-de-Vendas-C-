-- Criar a base de dados "projeto" se não existir
CREATE DATABASE IF NOT EXISTS projeto;

-- Selecionar a base de dados "projeto"
USE projeto;

-- Criar a tabela "cliente"
CREATE TABLE IF NOT EXISTS cliente (
    id INT AUTO_INCREMENT PRIMARY KEY,
    nome VARCHAR(100) NOT NULL,
    email VARCHAR(100) NOT NULL UNIQUE,
    senha VARCHAR(255) NOT NULL,
    cpf VARCHAR(14) NOT NULL UNIQUE,
    bairro VARCHAR(100),
    cep VARCHAR(8) NOT NULL,
    cidade VARCHAR(100),
    numero VARCHAR(10),
    uf CHAR(2),
    complemento VARCHAR(100),
    telefone VARCHAR(15)
);

-- Inserir um cliente com informações aleatórias
INSERT INTO cliente (nome, email, senha, cpf, bairro, cep, cidade, numero, uf, complemento, telefone)
VALUES ('João Silva', 'joao.silva@example.com', 'senhaSegura123', '12345678901', 'Centro', '12345678', 'São Paulo', '100', 'SP', 'Apto 101', '11999999999');