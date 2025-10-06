using System.Diagnostics;
using System.Text;

namespace YtDlpGui
{
    public partial class MainForm : Form
    {
        private TextBox txtUrls;
        private ComboBox cmbDownloadType;
        private ComboBox cmbFormat;
        private CheckBox chkThumbnail;
        private CheckBox chkEmbedThumbnail;
        private Button btnDownload;
        private TextBox txtOutput;
        private TextBox txtOutputPath;
        private Button btnBrowse;
        private ProgressBar progressBar;
        private Label lblStatus;

        public MainForm()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.Text = "yt-dlp GUI - YouTube Downloader";
            this.Width = 800;
            this.Height = 600;
            this.StartPosition = FormStartPosition.CenterScreen;

            // URL TextBox
            Label lblUrls = new Label
            {
                Text = "URL(s) - One per line (supports single videos, playlists):",
                Location = new Point(20, 20),
                AutoSize = true
            };
            this.Controls.Add(lblUrls);

            txtUrls = new TextBox
            {
                Location = new Point(20, 45),
                Size = new Size(740, 100),
                Multiline = true,
                ScrollBars = ScrollBars.Vertical,
                PlaceholderText = "https://www.youtube.com/watch?v=VIDEO_ID\nhttps://www.youtube.com/playlist?list=PLAYLIST_ID"
            };
            this.Controls.Add(txtUrls);

            // Output Path
            Label lblOutputPath = new Label
            {
                Text = "Output Path:",
                Location = new Point(20, 155),
                AutoSize = true
            };
            this.Controls.Add(lblOutputPath);

            txtOutputPath = new TextBox
            {
                Location = new Point(20, 180),
                Size = new Size(640, 25),
                Text = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyVideos), "yt-dlp")
            };
            this.Controls.Add(txtOutputPath);

            btnBrowse = new Button
            {
                Text = "Browse",
                Location = new Point(670, 178),
                Size = new Size(90, 28)
            };
            btnBrowse.Click += BtnBrowse_Click;
            this.Controls.Add(btnBrowse);

            // Download Type ComboBox
            Label lblDownloadType = new Label
            {
                Text = "Download Type:",
                Location = new Point(20, 215),
                AutoSize = true
            };
            this.Controls.Add(lblDownloadType);

            cmbDownloadType = new ComboBox
            {
                Location = new Point(20, 240),
                Size = new Size(200, 25),
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            cmbDownloadType.Items.AddRange(new object[] { "Video (Best)", "Audio Only", "Video + Audio (Best)", "Custom Format" });
            cmbDownloadType.SelectedIndex = 0;
            cmbDownloadType.SelectedIndexChanged += CmbDownloadType_SelectedIndexChanged;
            this.Controls.Add(cmbDownloadType);

            // Format ComboBox
            Label lblFormat = new Label
            {
                Text = "Format:",
                Location = new Point(240, 215),
                AutoSize = true
            };
            this.Controls.Add(lblFormat);

            cmbFormat = new ComboBox
            {
                Location = new Point(240, 240),
                Size = new Size(200, 25),
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            cmbFormat.Items.AddRange(new object[] { "mp4", "webm", "mkv", "avi", "flv", "mp3", "m4a", "opus", "wav" });
            cmbFormat.SelectedIndex = 0;
            this.Controls.Add(cmbFormat);

            // Thumbnail Checkbox
            chkThumbnail = new CheckBox
            {
                Text = "Download Thumbnail",
                Location = new Point(20, 280),
                AutoSize = true,
                Checked = false
            };
            this.Controls.Add(chkThumbnail);

            // Embed Thumbnail Checkbox
            chkEmbedThumbnail = new CheckBox
            {
                Text = "Embed Thumbnail (Audio/Video)",
                Location = new Point(20, 310),
                AutoSize = true,
                Checked = false
            };
            this.Controls.Add(chkEmbedThumbnail);

            // Download Button
            btnDownload = new Button
            {
                Text = "Download",
                Location = new Point(20, 350),
                Size = new Size(120, 35),
                BackColor = Color.FromArgb(0, 120, 215),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            btnDownload.Click += BtnDownload_Click;
            this.Controls.Add(btnDownload);

            // Progress Bar
            progressBar = new ProgressBar
            {
                Location = new Point(20, 395),
                Size = new Size(740, 25),
                Style = ProgressBarStyle.Marquee,
                Visible = false
            };
            this.Controls.Add(progressBar);

            // Status Label
            lblStatus = new Label
            {
                Text = "Ready",
                Location = new Point(20, 425),
                AutoSize = true,
                ForeColor = Color.Gray
            };
            this.Controls.Add(lblStatus);

            // Output TextBox
            Label lblOutput = new Label
            {
                Text = "Output Log:",
                Location = new Point(20, 450),
                AutoSize = true
            };
            this.Controls.Add(lblOutput);

            txtOutput = new TextBox
            {
                Location = new Point(20, 475),
                Size = new Size(740, 80),
                Multiline = true,
                ReadOnly = true,
                ScrollBars = ScrollBars.Vertical,
                BackColor = Color.Black,
                ForeColor = Color.LightGreen,
                Font = new Font("Consolas", 9)
            };
            this.Controls.Add(txtOutput);
        }

        private void CmbDownloadType_SelectedIndexChanged(object? sender, EventArgs e)
        {
            // Update format options based on download type
            if (cmbDownloadType.SelectedIndex == 1) // Audio Only
            {
                cmbFormat.Items.Clear();
                cmbFormat.Items.AddRange(new object[] { "mp3", "m4a", "opus", "wav", "flac" });
                cmbFormat.SelectedIndex = 0;
            }
            else // Video
            {
                cmbFormat.Items.Clear();
                cmbFormat.Items.AddRange(new object[] { "mp4", "webm", "mkv", "avi", "flv" });
                cmbFormat.SelectedIndex = 0;
            }
        }

        private void BtnBrowse_Click(object? sender, EventArgs e)
        {
            using (FolderBrowserDialog folderDialog = new FolderBrowserDialog())
            {
                folderDialog.Description = "Select output folder for downloads";
                folderDialog.SelectedPath = txtOutputPath.Text;

                if (folderDialog.ShowDialog() == DialogResult.OK)
                {
                    txtOutputPath.Text = folderDialog.SelectedPath;
                }
            }
        }

        private async void BtnDownload_Click(object? sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtUrls.Text))
            {
                MessageBox.Show("Please enter at least one URL.", "No URLs", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Disable controls during download
            SetControlsEnabled(false);
            progressBar.Visible = true;
            lblStatus.Text = "Downloading...";
            txtOutput.Clear();

            try
            {
                // Get URLs (one per line)
                var urls = txtUrls.Text.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries)
                                       .Select(u => u.Trim())
                                       .Where(u => !string.IsNullOrWhiteSpace(u))
                                       .ToList();

                // Create output directory if it doesn't exist
                Directory.CreateDirectory(txtOutputPath.Text);

                // Build yt-dlp command
                StringBuilder args = new StringBuilder();

                // Add output path
                args.Append($"-o \"{Path.Combine(txtOutputPath.Text, "%(title)s.%(ext)s")}\" ");

                // Add format based on download type
                switch (cmbDownloadType.SelectedIndex)
                {
                    case 0: // Video (Best)
                        args.Append($"-f \"bestvideo[ext={cmbFormat.Text}]+bestaudio/best[ext={cmbFormat.Text}]/best\" ");
                        break;
                    case 1: // Audio Only
                        args.Append($"-x --audio-format {cmbFormat.Text} ");
                        break;
                    case 2: // Video + Audio (Best)
                        args.Append($"-f \"bestvideo+bestaudio/best\" --merge-output-format {cmbFormat.Text} ");
                        break;
                    case 3: // Custom Format
                        args.Append($"-f \"best\" ");
                        break;
                }

                // Add thumbnail options
                if (chkThumbnail.Checked)
                {
                    args.Append("--write-thumbnail ");
                }

                if (chkEmbedThumbnail.Checked)
                {
                    args.Append("--embed-thumbnail ");
                }

                // Add URLs
                foreach (var url in urls)
                {
                    args.Append($"\"{url}\" ");
                }

                // Execute yt-dlp
                await ExecuteYtDlp(args.ToString());

                lblStatus.Text = "Download completed!";
                lblStatus.ForeColor = Color.Green;
                MessageBox.Show("Download completed successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                lblStatus.Text = "Error occurred";
                lblStatus.ForeColor = Color.Red;
                txtOutput.AppendText($"\r\nError: {ex.Message}\r\n");
                MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                progressBar.Visible = false;
                SetControlsEnabled(true);
            }
        }

        private async Task ExecuteYtDlp(string arguments)
        {
            await Task.Run(() =>
            {
                ProcessStartInfo startInfo = new ProcessStartInfo
                {
                    FileName = "yt-dlp",
                    Arguments = arguments,
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    CreateNoWindow = true
                };

                using (Process? process = Process.Start(startInfo))
                {
                    if (process != null)
                    {
                        process.OutputDataReceived += (sender, e) =>
                        {
                            if (!string.IsNullOrEmpty(e.Data))
                            {
                                this.Invoke((Action)(() =>
                                {
                                    txtOutput.AppendText(e.Data + Environment.NewLine);
                                    txtOutput.SelectionStart = txtOutput.Text.Length;
                                    txtOutput.ScrollToCaret();
                                }));
                            }
                        };

                        process.ErrorDataReceived += (sender, e) =>
                        {
                            if (!string.IsNullOrEmpty(e.Data))
                            {
                                this.Invoke((Action)(() =>
                                {
                                    txtOutput.AppendText("ERROR: " + e.Data + Environment.NewLine);
                                    txtOutput.SelectionStart = txtOutput.Text.Length;
                                    txtOutput.ScrollToCaret();
                                }));
                            }
                        };

                        process.BeginOutputReadLine();
                        process.BeginErrorReadLine();
                        process.WaitForExit();

                        if (process.ExitCode != 0)
                        {
                            throw new Exception($"yt-dlp exited with code {process.ExitCode}");
                        }
                    }
                }
            });
        }

        private void SetControlsEnabled(bool enabled)
        {
            txtUrls.Enabled = enabled;
            txtOutputPath.Enabled = enabled;
            btnBrowse.Enabled = enabled;
            cmbDownloadType.Enabled = enabled;
            cmbFormat.Enabled = enabled;
            chkThumbnail.Enabled = enabled;
            chkEmbedThumbnail.Enabled = enabled;
            btnDownload.Enabled = enabled;
        }
    }
}
