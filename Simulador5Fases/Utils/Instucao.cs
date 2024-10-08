public enum Opcode
{
  ADD,
  SUB,
  BEQ,
  LW,
  SW,
  NOOP,
  HALT
}

public class Instrucao
{
  public Opcode Opcode { get; set; }
  public string Oper1 { get; set; }
  public string Oper2 { get; set; }
  public string Oper3 { get; set; }
  public int Temp1 { get; set; }
  public int Temp2 { get; set; }
  public int Temp3 { get; set; }
  public bool Valida { get; set; }

  public Instrucao(Opcode opcode, string oper1, string oper2, string oper3, int temp1, int temp2, int temp3, bool valida)
  {
    this.Opcode = opcode;
    this.Oper1 = oper1;
    this.Oper2 = oper2;
    this.Oper3 = oper3;
    this.Temp1 = temp1;
    this.Temp2 = temp2;
    this.Temp3 = temp3;
    this.Valida = valida;
  }
}