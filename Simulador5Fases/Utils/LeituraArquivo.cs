using System;
using System.IO;

public class LeituraArquivo 
{
    public List<Instrucao> lerCase(bool option) {
        List<Instrucao> instrucoes = new List<Instrucao>();

        try
        {
            using (StreamReader reader = new StreamReader(option ? "txts/commands_1.txt" : "txts/commands_2.txt"))
            {
                string line;
        var index = 0;
        while ((line = reader.ReadLine()) != null)
        {
          index++;
          string[] list = line.Split(" ");

          Opcode opcode = (Opcode)Enum.Parse(typeof(Opcode), list[0], true);

          if (list.Length == 1)
          {
            instrucoes.Add(new(index, opcode));
            continue;
          }

          instrucoes.Add(new(index, opcode, list[1], list[2], list[3]));
        }
      }
    }
    catch (Exception ex)
    {
      Console.WriteLine($"Ocorreu um erro: {ex.Message}");
    }

    return instrucoes;
  }
}