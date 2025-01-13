using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace battleships_game_app.CellRelated
{
    public class Icon
    {
        private readonly Dictionary<Type, char> _icons;

        public Icon()
        {
            // Default icons for each state
            _icons = new Dictionary<Type, char>
            {
                { typeof(NotHit), '@' },
                { typeof(WasHit), 'X' },
                { typeof(Sunk), 'S' },
                { typeof(Neutral), '~' },
                { typeof(HitWater), 'O' }
            };
        }

        public char GetIconForState(ICellState state)
        {
            var stateType = state.GetType();
            return _icons.ContainsKey(stateType) ? _icons[stateType] : '?';
        }

        public void SetIconForState<TState>(char icon) where TState : ICellState
        {
            var stateType = typeof(TState);
            if (_icons.ContainsKey(stateType))
            {
                _icons[stateType] = icon;
            }
            else
            {
                _icons.Add(stateType, icon);
            }
        }
    }
}
