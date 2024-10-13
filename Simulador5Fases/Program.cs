Dictionary<string, int> bancoRegistradores = new Dictionary<string, int>();
for (int i = 0; i <= 32; i++) {
  if (i == 11)
  {
    bancoRegistradores["R11"] = -1;
    continue;
  }
  if (i == 7)
  {
    bancoRegistradores["R7"] = -9;
    continue;
  }
  bancoRegistradores[$"R{i}"] = i;
}

Pipeline pipeline = new Pipeline(bancoRegistradores);

pipeline.Main();
Console.WriteLine("teste");