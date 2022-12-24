namespace PrecisionMachineDesignFinalReport
{
    partial class Form1
    {
        /// <summary>
        /// 設計工具所需的變數。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清除任何使用中的資源。
        /// </summary>
        /// <param name="disposing">如果應該處置受控資源則為 true，否則為 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form 設計工具產生的程式碼

        /// <summary>
        /// 此為設計工具支援所需的方法 - 請勿使用程式碼編輯器修改
        /// 這個方法的內容。
        /// </summary>
        private void InitializeComponent()
        {
            this.button_Generatecsv = new System.Windows.Forms.Button();
            this.button_Generatemd = new System.Windows.Forms.Button();
            this.textBox_output = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // button_Generatecsv
            // 
            this.button_Generatecsv.Font = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.button_Generatecsv.Location = new System.Drawing.Point(13, 381);
            this.button_Generatecsv.Name = "button_Generatecsv";
            this.button_Generatecsv.Size = new System.Drawing.Size(372, 57);
            this.button_Generatecsv.TabIndex = 0;
            this.button_Generatecsv.Text = "產生.csv檔";
            this.button_Generatecsv.UseVisualStyleBackColor = true;
            this.button_Generatecsv.Click += new System.EventHandler(this.button_Generatecsv_Click);
            // 
            // button_Generatemd
            // 
            this.button_Generatemd.Font = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.button_Generatemd.Location = new System.Drawing.Point(416, 381);
            this.button_Generatemd.Name = "button_Generatemd";
            this.button_Generatemd.Size = new System.Drawing.Size(372, 57);
            this.button_Generatemd.TabIndex = 1;
            this.button_Generatemd.Text = "產生Markdown語法";
            this.button_Generatemd.UseVisualStyleBackColor = true;
            this.button_Generatemd.Click += new System.EventHandler(this.button_Generatemd_Click);
            // 
            // textBox_output
            // 
            this.textBox_output.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox_output.Location = new System.Drawing.Point(13, 13);
            this.textBox_output.Multiline = true;
            this.textBox_output.Name = "textBox_output";
            this.textBox_output.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textBox_output.Size = new System.Drawing.Size(775, 362);
            this.textBox_output.TabIndex = 2;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.textBox_output);
            this.Controls.Add(this.button_Generatemd);
            this.Controls.Add(this.button_Generatecsv);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button_Generatecsv;
        private System.Windows.Forms.Button button_Generatemd;
        private System.Windows.Forms.TextBox textBox_output;
    }
}

