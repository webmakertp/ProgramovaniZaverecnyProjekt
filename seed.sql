-- naplneni platformama, tohle je fixni seznam

INSERT INTO Platform (Name) VALUES
    ('PC'),
    ('PlayStation 5'),
    ('Xbox Series X'),
    ('Nintendo Switch'),
    ('PlayStation 4'),
    ('Xbox One')
ON CONFLICT DO NOTHING;
