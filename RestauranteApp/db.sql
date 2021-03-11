CREATE TABLE StatusPedido (
  StatusId int PRIMARY KEY,
  Descricao varchar(20) NOT NULL
);

CREATE TABLE Produto (
  ProdutoId int PRIMARY KEY,
  ImagemProduto varchar(255),
  NomeProduto varchar(255) NOT NULL,
  ValorProduto float NOT NULL,
  Disponivel boolean NOT NULL
);

CREATE TABLE Pedido (
  PedidoId int PRIMARY KEY,
  ProdutoId int NOT NULL,
  ComandaId int NOT NULL,
  ValorPedido float NOT NULL,
  QtdeProduto int NOT NULL,
  StatusPedido int NOT NULL
);

CREATE TABLE Mesa (
  MesaId int PRIMARY KEY,
  CapacidadePessoasMesa int NOT NULL,
  MesaOcupada boolean NOT NULL
);

CREATE TABLE Comanda (
  ComandaId int PRIMARY KEY,
  MesaId int NOT NULL,
  DataHoraEntrada datetime NOT NULL,
  DataHoraSaida datetime null,
  Valor float NOT NULL,
  ComandaPaga boolean,
  QtdePessoasMesa int NOT NULL
);

ALTER TABLE Pedido ADD CONSTRAINT FK_ComandaId FOREIGN KEY (ComandaId) REFERENCES Comanda(ComandaId);

ALTER TABLE Pedido ADD CONSTRAINT FK_ProdutoId FOREIGN KEY (ProdutoId) REFERENCES Produto(ProdutoId);

ALTER TABLE Pedido ADD CONSTRAINT FK_StatusId FOREIGN KEY (StatusPedido) REFERENCES StatusPedido(StatusId);

ALTER TABLE Comanda ADD CONSTRAINT FK_MesaId FOREIGN KEY (MesaId) REFERENCES Mesa(MesaId);


INSERT INTO Produto (ProdutoId, ImagemProduto, NomeProduto, ValorProduto, Disponivel)
   VALUES  (1, '', 'Rodizio', '70.00', 'True'),
           (2, '', 'Yakissoba', '0.00', 'True'),
           (3, '', 'Temaki', '0.00', 'True'),
           (4, '', 'Urumaki', '0.00', 'True'),
           (6, '', 'Hot Holl', '0.00', 'True'),
           (7, '', 'Sashimi', '0.00', 'True'),
           (8, '', 'Suco Natual', '10.00', 'True'),
           (9, '', 'Refrigerante Lata', '5.00', 'True'),
           (10, '', 'Sorvete', '15.00', 'True')

INSERT INTO Mesa (MesaId, CapacidadePessoasMesa, MesaOcupada)
    VALUES (1, 4, 'False'),
           (2, 4, 'False'),
           (3, 4, 'False'),
           (4, 4, 'False'),           
           (5, 4, 'False'),
           (6, 4, 'False'),
           (7, 4, 'False'),
           (8, 4, 'False'),
           (9, 4, 'False'),
           (10, 4, 'False'),
           (11, 4, 'False'),
           (12, 4, 'False'),
           (13, 4, 'False'),
           (14, 4, 'False'),
           (15, 4, 'False'),
           (16, 4, 'False')

INSERT INTO StatusPedido (StatusId, Descricao)
    VALUES (1, 'Entregue'),
           (2, 'Cancelado')