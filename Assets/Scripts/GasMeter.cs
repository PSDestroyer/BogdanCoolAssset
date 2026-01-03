
using GenesisStudio;

public class GasMeter
{
    private float _gas;
    private float _maxGas;

    public float Gas
    {
        get => _gas;
        set
        {
            _gas = value;
            if(_gas > _maxGas)
                _gas = _maxGas;
            
            if(_gas <= 0)
                _gas = 0;
            
            GameEventBus.Instance.OnGasChanged.Invoke(_gas);
        }
    }

    public GasMeter(float gas = 20,  float maxGas = 100f)
    {
        _gas = gas;
        _maxGas = maxGas;
    }
    
    
}
