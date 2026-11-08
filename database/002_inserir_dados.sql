INSERT INTO tb_condicao_jogo (co_condicao_jogo, ds_condicao_jogo) VALUES
(1, 'Novo'),
(2, 'Usado'),
(3, 'Recondicionado');

INSERT INTO tb_perfil (co_perfil, ds_perfil) VALUES
(1, 'Administrador'),
(2, 'Cliente');

INSERT INTO tb_plataforma (no_plataforma, no_fabricante, nu_ano_lancamento) VALUES
('Super Nintendo', 'Nintendo', 1990),
('Mega Drive', 'Sega', 1988),
('PlayStation', 'Sony', 1994),
('Nintendo 64', 'Nintendo', 1996),
('Game Boy', 'Nintendo', 1989),
('Sega Saturn', 'Sega', 1994);

INSERT INTO tb_genero (no_genero) VALUES
('Ação'), ('Aventura'), ('Plataforma'), ('RPG'),
('Corrida'), ('Luta'), ('Estratégia'), ('Esporte');

INSERT INTO tb_jogo (no_jogo, ds_jogo, nu_ano_lancamento, vl_preco, qt_estoque, st_ativo, dt_cadastro, co_plataforma, co_condicao_jogo) VALUES
('Super Mario World', 'Classico de plataforma.', 1990, 150.00, 5, TRUE, NOW(), 1, 2),
('The Legend of Zelda: A Link to the Past', 'Aventura epica.', 1991, 220.00, 3, TRUE, NOW(), 1, 2),
('Sonic the Hedgehog', 'Velocidade com Sonic.', 1991, 120.00, 8, TRUE, NOW(), 2, 2),
('Castlevania: Symphony of the Night', 'Aventura no castelo.', 1997, 450.00, 2, TRUE, NOW(), 3, 2),
('Super Mario 64', 'Mario em 3D.', 1996, 200.00, 4, TRUE, NOW(), 4, 2),
('Chrono Trigger', 'RPG de viagem no tempo.', 1995, 600.00, 1, TRUE, NOW(), 1, 2),
('Streets of Rage 2', 'Beat em up classico.', 1992, 130.00, 6, TRUE, NOW(), 2, 2),
('Final Fantasy VII', 'RPG memorável.', 1997, 300.00, 4, TRUE, NOW(), 3, 2),
('Pokémon Red', 'Aventura Pokémon.', 1996, 180.00, 0, TRUE, NOW(), 5, 2),
('GoldenEye 007', 'FPS revolucionario.', 1997, 160.00, 3, TRUE, NOW(), 4, 2),
('Donkey Kong Country', 'Plataforma visual incrivel.', 1994, 140.00, 7, TRUE, NOW(), 1, 2),
('Phantasy Star IV', 'RPG de ficção científica.', 1993, 350.00, 2, TRUE, NOW(), 2, 2),
('Resident Evil 2', 'Survival horror.', 1998, 250.00, 5, TRUE, NOW(), 3, 2),
('Super Smash Bros.', 'Luta de mascotes.', 1999, 210.00, 3, TRUE, NOW(), 4, 2),
('Tetris', 'Quebra-cabeça definitivo.', 1989, 90.00, 10, TRUE, NOW(), 5, 1),
('Nights into Dreams', 'Voe em mundos de sonho.', 1996, 190.00, 2, TRUE, NOW(), 6, 3),
('Street Fighter II Turbo', 'Luta de arcade.', 1993, 170.00, 4, TRUE, NOW(), 1, 2),
('Metal Gear Solid', 'Ação tatica.', 1998, 280.00, 3, TRUE, NOW(), 3, 2),
('Banjo-Kazooie', 'Plataforma 3D.', 1998, 190.00, 4, TRUE, NOW(), 4, 2),
('Virtual Racing (Inativo)', 'Jogo inativo de teste.', 1992, 50.00, 1, FALSE, NOW(), 2, 2);

INSERT INTO tb_jogo_genero (co_jogo, co_genero) VALUES
(1, 3), (2, 2), (2, 4), (3, 3), (3, 1), (4, 1), (4, 2), (5, 3), (5, 2),
(6, 4), (7, 1), (8, 4), (9, 4), (10, 1), (11, 3), (12, 4), (13, 1), (13, 2),
(14, 6), (15, 7), (16, 2), (17, 6), (18, 1), (18, 2), (19, 3), (19, 2), (20, 5);
