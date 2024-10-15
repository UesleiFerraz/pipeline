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
  public int Id { get; set; }
  public Opcode Opcode { get; set; }
  public string Oper1 { get; set; }
  public string Oper2 { get; set; }
  public string Oper3 { get; set; }
  public int DecodeOper1 { get; set; }
  public int DecodeOper2 { get; set; }
  public int DecodeOper3 { get; set; }
  public int Temp1 { get; set; }
  public int Temp2 { get; set; }
  public int Temp3 { get; set; }
  public bool Valida { get; set; } = true;

  public Instrucao(Opcode opcode, string oper1, string oper2, string oper3, int temp1, int temp2, int temp3, bool valida = true)
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

  public Instrucao(int id, Opcode opcode, string oper1, string oper2, string oper3, int decodeOper1, int decodeOper2, int decodeOper3, int temp1, int temp2, int temp3, bool valida = true)
  {
    this.Id = id;
    this.Opcode = opcode;
    this.Oper1 = oper1;
    this.Oper2 = oper2;
    this.Oper3 = oper3;
    this.DecodeOper1 = decodeOper1;
    this.DecodeOper2 = decodeOper2;
    this.DecodeOper3 = decodeOper3;
    this.Temp1 = temp1;
    this.Temp2 = temp2;
    this.Temp3 = temp3;
    this.Valida = valida;
  }

  public Instrucao(int id, Opcode opcode, string oper1, string oper2, string oper3)
  {
    this.Id = id;
    this.Opcode = opcode;
    this.Oper1 = oper1;
    this.Oper2 = oper2;
    this.Oper3 = oper3;
  }

  public Instrucao(int id, Opcode opcode)
  {
    this.Id = id;
    this.Opcode = opcode;
  }

  public Instrucao Clonar() {
    return new Instrucao(this.Id, this.Opcode, this.Oper1, this.Oper2, this.Oper3, this.DecodeOper1, this.DecodeOper2, this.DecodeOper3, this.Temp1, this.Temp2, this.Temp3, this.Valida);
  }
}