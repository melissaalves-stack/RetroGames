-- 1. Catálogo: jogos ativos ordenados por nome
SELECT j.no_jogo, j.vl_preco, j.qt_estoque 
FROM tb_jogo j 
WHERE j.st_ativo = TRUE 
ORDER BY j.no_jogo;

-- 2. Quantidade de jogos por plataforma
SELECT p.no_plataforma, COUNT(j.co_seq_jogo) AS total_jogos
FROM tb_plataforma p
LEFT JOIN tb_jogo j ON p.co_seq_plataforma = j.co_plataforma
GROUP BY p.co_seq_plataforma, p.no_plataforma;
