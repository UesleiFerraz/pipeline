Dictionary<string, int> bancoRegistradores = new Dictionary<string, int>();
for (int i = 0; i <= 32; i++) {
  bancoRegistradores[$"R{i}"] = i;
}

Pipeline pipeline = new Pipeline(bancoRegistradores);

pipeline.Main();
Console.WriteLine("teste");