using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace advent._2019._11
{
    public class HullPainter
    {
        private readonly _2.IntComputer _computer;
        private readonly long[] _program;
        private readonly ChannelReader<long> _instructions;
        private readonly ChannelWriter<long> _sensor;

        private readonly HashSet<(int, int)> _whitePanels = new HashSet<(int, int)>();
        private readonly HashSet<(int, int)> _paintedPanels = new HashSet<(int, int)>();
        private int _x;
        private int _y;
        private Direction _dir;

        public HullPainter(long[] program)
        {
            _program = program;
            var input = Channel.CreateUnbounded<long>();
            var output = Channel.CreateUnbounded<long>();
            _computer = new _2.IntComputer(input.Reader, output.Writer);
            _sensor = input.Writer;
            _instructions = output.Reader;
        }

        public async Task RunProgram(bool firstPanelWhite = false)
        {
            await Task.WhenAll(
                _computer.RunProgram(_program).AsTask(),
                PaintUntilDone(firstPanelWhite).AsTask());
        }

        private async ValueTask PaintUntilDone(bool firstPanelWhite)
        {
            if (firstPanelWhite) { _whitePanels.Add((_x, _y)); }
            while (await PaintOnce()) { }
        }

        public async ValueTask<bool> PaintOnce()
        {
            var color = _whitePanels.Contains((_x, _y)) ? 1 : 0;
            await _sensor.WriteAsync(color);
            if (!(await _instructions.WaitToReadAsync())) { return false; }
            var newColor = await _instructions.ReadAsync();
            PaintSquare(newColor);
            var newDirection = await _instructions.ReadAsync();
            Turn(newDirection);
            MoveForward();
            return true;
        }

        public void PaintSquare(long color)
        {
            switch (color)
            {
                case 0:
                    _whitePanels.Remove((_x, _y));
                    break;
                case 1:
                    _whitePanels.Add((_x, _y));
                    break;
                default:
                    throw new ArgumentException($"paint instruction: {color}");
            }
            _paintedPanels.Add((_x, _y));
        }

        public void Turn(long direction)
        {
            var incr = direction switch
            {
                0 => -1,
                1 => 1,
                _ => throw new ArgumentException($"turn instruction: {direction}")
            };
            var newDir = (int)_dir + incr;
            if (newDir < 0) { newDir += 4; }
            _dir = (Direction)(newDir % 4);
        }

        public void MoveForward()
        {
            var (dx, dy) = _dir switch
            {
                Direction.Up => (0, -1),
                Direction.Left => (-1, 0),
                Direction.Down => (0, 1),
                Direction.Right => (1, 0),
                _ => throw new ArgumentException($"move direction: {_dir}")
            };
            _x += dx;
            _y += dy;
        }

        public int PaintedPanels => _paintedPanels.Count;

        public string Render()
        {
            int minX = int.MaxValue, minY = int.MaxValue;
            int maxX = int.MinValue, maxY = int.MinValue;
            foreach (var (x, y) in _whitePanels)
            {
                if (x < minX) { minX = x; }
                if (x > maxX) { maxX = x; }
                if (y < minY) { minY = y; }
                if (y > maxY) { maxY = y; }
            }
            var sb = new StringBuilder();
            for (int y = minY; y <= maxY; y++)
            {
                for (int x = minX; x <= maxX; x++)
                {
                    var c = _whitePanels.Contains((x, y)) ? '#' : '.';
                    sb.Append(c);
                }
                sb.Append(Environment.NewLine);
            }
            return sb.ToString();
        }

        public static async ValueTask<HullPainter> Create(
            IAsyncEnumerable<string> lines)
        {
            var line = await lines.Single();
            var program = _2.IntComputer.ParseLine(line);
            return new HullPainter(program);
        }
    }

    public enum Direction { Up, Right, Down, Left }
}
