CREATE TABLE StatusPedido (
  StatusPedidoId int PRIMARY KEY,
  Descricao varchar(20) NOT NULL
);

CREATE TABLE Produto (
  ProdutoId int PRIMARY KEY,
  ImagemProduto varchar(255),
  NomeProduto varchar(255) NOT NULL,
  ValorProduto float NOT NULL,
  QtdePermitida int NOT NULL,
  Disponivel bit NOT NULL
);

CREATE TABLE Pedido (
  PedidoId int PRIMARY KEY,
  ProdutoId int NOT NULL,
  ComandaId int NOT NULL,
  ValorPedido float NOT NULL,
  QtdeProduto int NOT NULL,
  StatusPedidoId int NOT NULL,
  DataHoraPedido datetime NOT NULL
);

CREATE TABLE Mesa (
  MesaId int PRIMARY KEY,
  Capacidade int NOT NULL,
  MesaOcupada bit NOT NULL
);

CREATE TABLE Comanda (
  ComandaId int PRIMARY KEY,
  MesaId int NOT NULL,
  DataHoraEntrada datetime NOT NULL,
  DataHoraSaida datetime null,
  Valor float NOT NULL,
  ComandaPaga bit,
  QtdePessoasMesa int NOT NULL
);

ALTER TABLE Pedido ADD CONSTRAINT FK_ComandaId FOREIGN KEY (ComandaId) REFERENCES Comanda(ComandaId);

ALTER TABLE Pedido ADD CONSTRAINT FK_ProdutoId FOREIGN KEY (ProdutoId) REFERENCES Produto(ProdutoId);

ALTER TABLE Pedido ADD CONSTRAINT FK_StatusPedidoId FOREIGN KEY (StatusPedidoId) REFERENCES StatusPedido(StatusPedidoId);

ALTER TABLE Comanda ADD CONSTRAINT FK_MesaId FOREIGN KEY (MesaId) REFERENCES Mesa(MesaId);


INSERT INTO Produto (ProdutoId, ImagemProduto, NomeProduto, ValorProduto, QtdePermitida, Disponivel)
   VALUES  (1, 'https://images.unsplash.com/photo-1553944384-ffdc4fd8f2fa?ixid=MnwxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8&ixlib=rb-1.2.1&auto=format&fit=crop&w=1351&q=80', 'Rodizio', '70.00', 4, 'True'),
           (2, 'https://cdn.shortpixel.ai/client/to_avif,q_glossy,ret_img,w_620/https://blog.bomsabor.com.br/wp-content/uploads/2017/09/download.jpg', 'Yakissoba', '0.00', 2, 'True'),
           (3, 'https://cheftime-bucket.s3.sa-east-1.amazonaws.com/storage/imported/temaki-de-salmao-com-cream-cheese-e-cebolinha-imagem-menu-destaque-7f202c2ae7%402x.jpg', 'Temaki', '0.00', 4, 'True'),
           (4, 'https://lojanakayoshi.com.br/wp-content/uploads/2016/11/19-Sushi-de-Salm%C3%A3o-Grelhado-G.jpg', 'Urumaki', '0.00', 8, 'True'),
           (6, 'https://blogsakura.com.br/wp-content/uploads/2016/09/BLOG_receitas_hotroll.jpg', 'Hot Holl', '0.00', 8, 'True'),
           (7, 'https://www.djapa.com.br/wp-content/uploads/2020/07/peixe-cru.jpg', 'Sashimi', '0.00', 8, 'True'),
           (8, 'https://www.receiteria.com.br/wp-content/uploads/receitas-de-suco-1200x774.jpg', 'Suco Natural', '10.00', 4, 'True'),
           (9, 'https://boomburgers.com.br/wp-content/uploads/2020/08/refrigerantes-lata-350ml-min.jpg', 'Refrigerante Lata', '5.00', 4, 'True'),
           (10, 'https://img.itdg.com.br/tdg/images/blog/uploads/2019/01/Casquinha-de-sorvete.jpg', 'Sorvete', '15.00', 4, 'True')

INSERT INTO Mesa (MesaId, Capacidade, MesaOcupada)
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

INSERT INTO StatusPedido (StatusPedidoId, Descricao)
    VALUES (1, 'Entregue'),
           (2, 'Cancelado')