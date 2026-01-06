
using System.Collections;
using GenesisStudio;
using UnityEngine;

public class GasMeter
{
    private float _gas;
    private float _maxGas;
    private float _refillSpeed;    
    
    
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
            
            GameEventBus.Instance.OnGasChanged?.Invoke(_gas);
        }
    }

    public GasMeter(float gas = 20,  float maxGas = 100f)
    {
        _gas = gas;
        _maxGas = maxGas;
        _refillSpeed = 5;
    }
    

    public IEnumerator Refill()
    {
        while(Gas <= _maxGas)
        {
            Gas += _refillSpeed * Time.deltaTime;
            yield return null;
        }
    }

}
