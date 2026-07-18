public abstract class EstadoBase
{
    protected NPCHeridoFSM fsm;
    protected NPCHerido npc;

    protected EstadoBase(NPCHeridoFSM fsm, NPCHerido npc)
    {
        this.fsm = fsm;
        this.npc = npc;
    }

    public virtual void Entrar() { }

    public virtual void Actualizar() { }

    public virtual void Salir() { }
}