CREATE DATABASE IF NOT EXISTS projeto;

USE projeto;

CREATE TABLE funcionario (
    id INT AUTO_INCREMENT PRIMARY KEY,
    nome VARCHAR(100) NOT NULL COMMENT 'Nome completo do funcionário',
    email VARCHAR(100) NOT NULL UNIQUE COMMENT 'Endereço de e-mail do funcionário',
    senha VARCHAR(255) NOT NULL COMMENT 'Senha hash do funcionário',
    cpf VARCHAR(14) NOT NULL UNIQUE COMMENT 'Número do CPF do funcionário',
    bairro VARCHAR(100) COMMENT 'Bairro onde o funcionário reside',
    cep VARCHAR(10) COMMENT 'CEP do endereço do funcionário',
    cidade VARCHAR(100) COMMENT 'Cidade onde o funcionário reside',
    numero VARCHAR(10) COMMENT 'Número do endereço do funcionário',
    uf CHAR(2) COMMENT 'Sigla do estado onde o funcionário reside',
    carteira VARCHAR(20) COMMENT 'Número da carteira de trabalho do funcionário',
    telefone VARCHAR(20) COMMENT 'Número de telefone do funcionário',
    cargo VARCHAR(50) COMMENT 'Cargo do funcionário',
    aniversario VARCHAR(10) COMMENT 'Data de nascimento do funcionário',
    nivel_acesso INT COMMENT 'Nível de acesso do funcionário',
    salario DECIMAL(10, 2) COMMENT 'Salário do funcionário',

    INDEX idx_nome (nome),
    INDEX idx_email (email)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;
