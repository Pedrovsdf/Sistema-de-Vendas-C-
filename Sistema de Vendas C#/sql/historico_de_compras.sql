CREATE DATABASE IF NOT EXISTS projeto;
USE projeto;

-- Cria a tabela de histórico de compras
CREATE TABLE historico_compras (
    id_compra INT AUTO_INCREMENT PRIMARY KEY,
    cpf_cliente VARCHAR(14) NOT NULL COMMENT 'CPF do cliente que realizou a compra',
    data_compra DATETIME NOT NULL COMMENT 'Data e hora da compra',
    valor_total DECIMAL(10, 2) NOT NULL COMMENT 'Valor total da compra',
    dinheiro_recebido DECIMAL(10, 2) NOT NULL COMMENT 'Quantidade de dinheiro recebida do cliente',
    troco DECIMAL(10, 2) NOT NULL COMMENT 'Valor do troco devolvido ao cliente',
    lista_itens JSON NOT NULL COMMENT 'Lista de itens comprados, armazenada como JSON'
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

-- Insere um registro de exemplo na tabela de histórico de compras
INSERT INTO historico_compras (
    cpf_cliente, data_compra, valor_total, dinheiro_recebido, troco, lista_itens
) VALUES (
    '12345678901', -- CPF do cliente
    NOW(), -- Data e hora atuais
    150.75, -- Valor total da compra
    200.00, -- Dinheiro recebido
    49.25, -- Troco
    '[{"item": "Item1", "quantidade": 2}, {"item": "Item2", "quantidade": 1}, {"item": "Item3", "quantidade": 3}]' -- Lista de itens comprados em formato JSON
);