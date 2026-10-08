namespace bai4
{
    public partial class Form1 : Form
    {
        private const int Rows = 4;
        private const int Cols = 5;
        private readonly Button[,] seatButtons = new Button[Rows, Cols];
        private readonly HashSet<Button> selectedSeats = new();

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            cboTimeSlot.SelectedIndex = 0;

            var random = new Random();
            for (int r = 0; r < Rows; r++)
            {
                for (int c = 0; c < Cols; c++)
                {
                    if (tableSeats.GetControlFromPosition(c, r) is not Button btn) continue;

                    btn.Tag = false;
                    if (random.Next(0, 5) == 0)
                    {
                        btn.BackColor = Color.IndianRed;
                        btn.Enabled = false;
                    }

                    btn.Click += Seat_Click;
                    seatButtons[r, c] = btn;
                }
            }
        }

        private void Seat_Click(object? sender, EventArgs e)
        {
            if (sender is not Button btn) return;

            bool isSelected = btn.Tag is true;
            if (isSelected)
            {
                btn.BackColor = Color.LightGray;
                btn.Tag = false;
                selectedSeats.Remove(btn);
            }
            else
            {
                btn.BackColor = Color.LightGreen;
                btn.Tag = true;
                selectedSeats.Add(btn);
            }

            UpdateSummary();
        }

        private void cboTimeSlot_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateSummary();
        }

        private void UpdateSummary()
        {
            int price = cboTimeSlot.SelectedIndex == 0 ? 100_000 : 150_000;
            lblSelected.Text = $"Số vị trí đang chọn: {selectedSeats.Count}";
            lblTotal.Text = $"Tạm tính tiền: {selectedSeats.Count * price:N0}đ";
        }

        private void btnConfirm_Click(object sender, EventArgs e)
        {
            if (selectedSeats.Count == 0)
            {
                MessageBox.Show("Vui lòng chọn ít nhất một vị trí!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var seats = string.Join(", ", selectedSeats.Select(b => b.Text));
            int price = cboTimeSlot.SelectedIndex == 0 ? 100_000 : 150_000;
            string slot = cboTimeSlot.SelectedIndex == 0 ? "Sáng" : "Tối";

            MessageBox.Show(
                $"Đặt thành công!\nVị trí: {seats}\nKhung giờ: {slot}\nTổng tiền: {selectedSeats.Count * price:N0}đ",
                "Xác nhận", MessageBoxButtons.OK, MessageBoxIcon.Information);

            foreach (var btn in selectedSeats)
            {
                btn.BackColor = Color.IndianRed;
                btn.Enabled = false;
                btn.Tag = false;
            }
            selectedSeats.Clear();
            UpdateSummary();
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            foreach (var btn in selectedSeats)
            {
                btn.BackColor = Color.LightGray;
                btn.Tag = false;
            }
            selectedSeats.Clear();
            UpdateSummary();
        }
    }
}