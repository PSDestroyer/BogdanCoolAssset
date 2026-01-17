using UnityEngine;

namespace GenesisStudio
{
    public interface IGrabable
    {
        public void Grab(Transform hands);
        public void Release();
    }
}