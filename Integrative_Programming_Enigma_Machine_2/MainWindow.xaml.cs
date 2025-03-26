using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Net.NetworkInformation;
using System;
using System.Security.Cryptography;
using System.Windows.Media;

namespace Integrative_Programming_Enigma_Machine_2
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        // Constants for rotor wiring and reflector
        public string _control = "ABCDEFGHIJKLMNOPQRSTUVWXYZ"; // Standard alphabet for reference
        public string _ring1 = "DMTWSILRUYQNKFEJCAZBPGXOHV"; // Rotor 1 wiring (Hours)
        public string _ring2 = "HQZGPJTMOBLNCIFDYAWVEUSRKX"; // Rotor 2 wiring (Minutes)
        public string _ring3 = "UQNTLSZFMREHDPXKIBVYGJCWOA"; // Rotor 3 wiring (Seconds)
        public string _reflector = "YRUHQSLDPXNGOKMIEBFZCWVJAT"; // Reflector wiring

        // Rotor offset tracking
        public int[] _keyOffset = { 0, 0, 0 }; // Current rotor offsets (H, M, S)
        public int[] _initOffset = { 0, 0, 0 }; // Initial rotor offsets (H, M, S)

        // Rotor state flag
        public bool _rotor = false;

        // Plugboard setup
        public Dictionary<char, char> _plugboard = new Dictionary<char, char>(); // Plugboard dictionary
        private bool _plugboardSet = false; // Flag to indicate if plugboard is set

        // Constructor
        public MainWindow()
        {
            InitializeComponent();
            SetDefaults(); // Initialize default values
            _rotor = false; // Initially rotor is off
            btnRotor.Content = "Rotor On"; // Set button text
            btnRotor.IsEnabled = false; // Disable rotor button until plugboard is set
            KeyDeactivate();
        }

        private void KeyDeactivate()
        {
            // ADDITIONAL : Putting the buttons in the array
            btnMKey.IsEnabled = false;
            btnQKey.IsEnabled = false;
            btnWKey.IsEnabled = false;
            btnEKey.IsEnabled = false;
            btnRKey.IsEnabled = false;
            btnTKey.IsEnabled = false;
            btnYKey.IsEnabled = false;
            btnUKey.IsEnabled = false;
            btnIKey.IsEnabled = false;
            btnOKey.IsEnabled = false;
            btnPKey.IsEnabled = false;
            btnAKey.IsEnabled = false;
            btnSKey.IsEnabled = false;
            btnDKey.IsEnabled = false;
            btnFKey.IsEnabled = false;
            btnGKey.IsEnabled = false;
            btnHKey.IsEnabled = false;
            btnJKey.IsEnabled = false;
            btnKKey.IsEnabled = false;
            btnLKey.IsEnabled = false;
            btnZKey.IsEnabled = false;
            btnXKey.IsEnabled = false;
            btnCKey.IsEnabled = false;
            btnVKey.IsEnabled = false;
            btnBKey.IsEnabled = false;
            btnNKey.IsEnabled = false;
           
        }

        private void KeyActivate()
        {
            // ADDITIONAL : Putting the buttons in the array
            btnMKey.IsEnabled = true;
            btnQKey.IsEnabled = true;
            btnWKey.IsEnabled = true;
            btnEKey.IsEnabled = true;
            btnRKey.IsEnabled = true;
            btnTKey.IsEnabled = true;
            btnYKey.IsEnabled = true;
            btnUKey.IsEnabled = true;
            btnIKey.IsEnabled = true;
            btnOKey.IsEnabled = true;
            btnPKey.IsEnabled = true;
            btnAKey.IsEnabled = true;
            btnSKey.IsEnabled = true;
            btnDKey.IsEnabled = true;
            btnFKey.IsEnabled = true;
            btnGKey.IsEnabled = true;
            btnHKey.IsEnabled = true;
            btnJKey.IsEnabled = true;
            btnKKey.IsEnabled = true;
            btnLKey.IsEnabled = true;
            btnZKey.IsEnabled = true;
            btnXKey.IsEnabled = true;
            btnCKey.IsEnabled = true;
            btnVKey.IsEnabled = true;
            btnBKey.IsEnabled = true;
            btnNKey.IsEnabled = true;
            
        }

        // Display rotor wiring in UI labels
        private void DisplayRing(Label displayLabel, string ring)
        {
            string temp = "";
            foreach (char r in ring)
                temp += r + "\t"; // Add tab for spacing
            displayLabel.Content = temp;
        }

        // Find the index of a character in a string
        private int IndexSearch(string ring, char letter)
        {
            int index = 0;
            for (int x = 0; x < ring.Length; x++)
            {
                if (ring[x] == letter)
                {
                    index = x;
                    break;
                }
            }
            return index;
        }

        
        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            //// Check for uppercase letters and message length
            //if (lblInput.Content.ToString().Length < 128)
            //{
            //    lblInput.Content += letterInput; // Append input character
            //    lblEncrpyt.Content += Encrypt(letterInput[0]) + ""; // Encrypt and append
            //    lblEncrpytMirror.Content += Mirror(letterInput[0]) + ""; // Mirror and append

            //    // Rotate rotors if enabled
            //    if (_rotor)
            //    {
            //        Rotate(true);
            //        DisplayRing(lblRing1, _ring1); // Update rotor display
            //        DisplayRing(lblRing2, _ring2);
            //        DisplayRing(lblRing3, _ring3);
            //    }

            //}
            // Handle space key
            if (e.Key == Key.Space)
            {
                lblInput.Content += " ";
                lblEncrpyt.Content += " ";
                lblEncrpytMirror.Content += " ";
            }
            // Handle backspace key
            else if (e.Key == Key.Back)
            {
                Rotate(false); // Rotate rotors backward
                DisplayRing(lblRing1, _ring1);
                DisplayRing(lblRing2, _ring2);
                DisplayRing(lblRing3, _ring3);

                lblInput.Content = RemoveLastLetter(lblInput.Content.ToString()); // Remove last character
                lblEncrpyt.Content = RemoveLastLetter(lblEncrpyt.Content.ToString());
                lblEncrpytMirror.Content = RemoveLastLetter(lblEncrpytMirror.Content.ToString());
            }
        }

        //private void Window_KeyDown(object sender, KeyEventArgs e)
        //{
        //    // Check for uppercase letters and message length
        //    if (e.Key.ToString().Length == 1 && lblInput.Content.ToString().Length < 128)
        //    {
        //        if ((int)e.Key.ToString()[0] >= 65 && (int)e.Key.ToString()[0] <= 90)
        //        {
        //            lblInput.Content += e.Key.ToString(); // Append input character
        //            lblEncrpyt.Content += Encrypt(e.Key.ToString()[0]) + ""; // Encrypt and append
        //            lblEncrpytMirror.Content += Mirror(e.Key.ToString()[0]) + ""; // Mirror and append

        //            // Rotate rotors if enabled
        //            if (_rotor)
        //            {
        //                Rotate(true);
        //                DisplayRing(lblRing1, _ring1); // Update rotor display
        //                DisplayRing(lblRing2, _ring2);
        //                DisplayRing(lblRing3, _ring3);
        //            }
        //        }
        //    }
        //    // Handle space key
        //    else if (e.Key == Key.Space)
        //    {
        //        lblInput.Content += " ";
        //        lblEncrpyt.Content += " ";
        //        lblEncrpytMirror.Content += " ";
        //    }
        //    // Handle backspace key
        //    else if (e.Key == Key.Back)
        //    {
        //        Rotate(false); // Rotate rotors backward
        //        DisplayRing(lblRing1, _ring1);
        //        DisplayRing(lblRing2, _ring2);
        //        DisplayRing(lblRing3, _ring3);

        //        lblInput.Content = RemoveLastLetter(lblInput.Content.ToString()); // Remove last character
        //        lblEncrpyt.Content = RemoveLastLetter(lblEncrpyt.Content.ToString());
        //        lblEncrpytMirror.Content = RemoveLastLetter(lblEncrpytMirror.Content.ToString());
        //    }
        //}

        // Encrypt a character
        private char Encrypt(char letter)
        {
            char newChar = letter;

            // Plugboard pass (before rotors)
            if (_plugboard.ContainsKey(newChar))
                newChar = _plugboard[newChar];
            else if (_plugboard.ContainsValue(newChar))
                newChar = _plugboard.FirstOrDefault(x => x.Value == newChar).Key;

            // Rotor pass forward
            newChar = _ring1[IndexSearch(_control, newChar)];
            newChar = _ring2[IndexSearch(_control, newChar)];
            newChar = _ring3[IndexSearch(_control, newChar)];

            // Reflector pass
            newChar = _reflector[IndexSearch(_control, newChar)];

            // Rotor pass backward
            newChar = _ring3[IndexSearch(_control, newChar)];
            newChar = _ring2[IndexSearch(_control, newChar)];
            newChar = _ring1[IndexSearch(_control, newChar)];

            // Plugboard pass (after rotors)
            if (_plugboard.ContainsKey(newChar))
                newChar = _plugboard[newChar];
            else if (_plugboard.ContainsValue(newChar))
                newChar = _plugboard.FirstOrDefault(x => x.Value == newChar).Key;

            return newChar;
        }

        // Mirror a character (encrypt and pass back through rotors)
        private char Mirror(char letter)
        {
            char newChar = Encrypt(letter);

            newChar = _ring3[IndexSearch(_control, newChar)];
            newChar = _ring2[IndexSearch(_control, newChar)];
            newChar = _ring1[IndexSearch(_control, newChar)];

            return newChar;
        }

        // Set default values
        private void SetDefaults()
        {
            _control = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            _ring1 = "DMTWSILRUYQNKFEJCAZBPGXOHV";
            _ring2 = "HQZGPJTMOBLNCIFDYAWVEUSRKX";
            _ring3 = "UQNTLSZFMREHDPXKIBVYGJCWOA";
            _keyOffset = new int[] { 0, 0, 0 };

            lblInput.Content = "";
            lblEncrpyt.Content = "";
            lblEncrpytMirror.Content = "";

            DisplayRing(lblControlRing, _control);
            DisplayRing(lblRing1, _ring1);
            DisplayRing(lblRing2, _ring2);
            DisplayRing(lblRing3, _ring3);
        }

        // Rotate rotors
        private void Rotate(bool forward)
        {
            if (forward)
            {
                _keyOffset[0]++;
                _ring1 = MoveValues(forward, _ring1);

                if (_keyOffset[0] / _control.Length >= 1)
                {
                    _keyOffset[0] = 2;
                    _keyOffset[1]++;
                    _ring2 = MoveValues(forward, _ring2);
                    if (_keyOffset[1] / _control.Length >= 1)
                    {
                        _keyOffset[1] = 2;
                        _keyOffset[2]++;
                        _ring3 = MoveValues(forward, _ring3);
                    }
                }
            }
            //else
            //{
            //    if (_keyOffset[2] > 0 || _keyOffset[1] > 0)
            //    {
            //        _keyOffset[2]--;
            //        _ring3 = MoveValues(forward, _ring3);
            //        if (_keyOffset[2] < 0)
            //        {
            //            _keyOffset[2] = 25;
            //            _keyOffset[1]--;
            //            _ring2 = MoveValues(forward, _ring2);
            //            if (_keyOffset[1] < 0)
            //            {
            //                _keyOffset[1] = 25;
            //                _keyOffset[0]--;
            //                _ring1 = MoveValues(forward, _ring1);
            //                if (_keyOffset[0] < 0)
            //                    _keyOffset[0] = 25;
            //            }
            //        }
            //    }
            //}

            DisplayOffset(); // Update offset display
        }

        // Move rotor values
        private string MoveValues(bool forward, string ring)
        {
            char movingValue = ' ';
            string newRing = "";

            if (forward)
            {
                movingValue = ring[0];
                for (int x = 1; x < ring.Length; x++)
                    newRing += ring[x];
                newRing += movingValue;
            }
            else
            {
                movingValue = ring[25];
                for (int x = 0; x < ring.Length - 1; x++)
                    newRing += ring[x];
                newRing = movingValue + newRing;
            }

            return newRing;
        }

        // Handle rotor button click
        private void btnRotor_Click(object sender, RoutedEventArgs e)
        {
            SetDefaults();

            if (int.TryParse(txtBRing1Init.Text, out _initOffset[0]) &&
                int.TryParse(txtBRing2Init.Text, out _initOffset[1]) &&
                int.TryParse(txtBRing3Init.Text, out _initOffset[2]))
            {
                if (_initOffset[0] >= 0 && _initOffset[0] <= 25 &&
                    _initOffset[1] >= 0 && _initOffset[1] <= 25 &&
                    _initOffset[2] >= 0 && _initOffset[2] <= 25)
                {
                    txtBRing1Init.IsEnabled = false;
                    txtBRing2Init.IsEnabled = false;
                    txtBRing3Init.IsEnabled = false;

                    _rotor = true;
                    btnRotor.Content = "Settings Lock";

                    _ring1 = InitializeRotors(_initOffset[0], _ring1);
                    _ring2 = InitializeRotors(_initOffset[1], _ring2);
                    _ring3 = InitializeRotors(_initOffset[2], _ring3);

                    DisplayRing(lblRing1, _ring1);
                    DisplayRing(lblRing2, _ring2);
                    DisplayRing(lblRing3, _ring3);
                    DisplayOffset();
                }
            }

            KeyActivate();

        }

        // Initialize rotors with initial offset
        private string InitializeRotors(int initial, string ring)
        {
            string newRing = ring;
            for (int x = 0; x < initial; x++)
                newRing = MoveValues(true, newRing);
            return newRing;
        }

        // Remove last letter from a string
        private string RemoveLastLetter(string word)
        {
            string newWord = "";
            for (int x = 0; x < word.Length - 1; x++)
                newWord += word[x];
            return newWord;
        }

        // Handle text box focus
        private void txtBRing1Init_GotFocus(object sender, RoutedEventArgs e)
        {
            txtBRing1Init.Text = "";
        }

        private void txtBRing2Init_GotFocus(object sender, RoutedEventArgs e)
        {
            txtBRing2Init.Text = "";
        }

        private void txtBRing3Init_GotFocus(object sender, RoutedEventArgs e)
        {
            txtBRing3Init.Text = "";
        }

        // Display rotor offsets
        private void DisplayOffset()
        {
            txtBRing1Init.Text = _keyOffset[0] + "";
            txtBRing2Init.Text = _keyOffset[1] + "";
            txtBRing3Init.Text = _keyOffset[2] + "";
        }

        // Setup plugboard
        private void SetupPlugboard(string plugboardSettings)
        {
            _plugboard.Clear();
            string[] pairs = plugboardSettings.ToUpper().Split(' ');
            foreach (string pair in pairs)
            {
                if (pair.Length == 2)
                {
                    _plugboard[pair[0]] = pair[1];
                    _plugboard[pair[1]] = pair[0];
                }
            }
        }

        // Handle plugboard button click
        private void btnSetPlugboard_Click(object sender, RoutedEventArgs e)
        {
            if (_plugboardSet)
            {
                MessageBox.Show("Plugboard is already set.");
                return;
            }

            SetupPlugboard(txtPlugboard.Text);
            _plugboardSet = true;
            btnRotor.IsEnabled = true; // Enable the rotor button.

            // Explicitly check the flag and perform an action
            if (_plugboardSet)
            {
                txtPlugboard.IsEnabled = false;
            }
        }

        // Handle plugboard text change
        private void txtPlugboard_TextChanged(object sender, TextChangedEventArgs e)
        {
            lblInput.Content = "";
            lblEncrpyt.Content = "";
            lblEncrpytMirror.Content = "";
        }

        private void Input(string letterInput)
        {
            // Check for uppercase letters and message length
            if (lblInput.Content.ToString().Length < 120)
            {
                lblInput.Content += letterInput; // Append input character
                char encryptedLetter = ' ';
                encryptedLetter = Encrypt(letterInput[0]); // Encrypt and append
                lblEncrpyt.Content += encryptedLetter + ""; // Encrypt and append
                BulbOn(encryptedLetter.ToString());
                lblEncrpytMirror.Content += Mirror(letterInput[0]) + ""; // Mirror and append

                // Rotate rotors if enabled
                if (_rotor)
                {
                    Rotate(true);
                    DisplayRing(lblRing1, _ring1); // Update rotor display
                    DisplayRing(lblRing2, _ring2);
                    DisplayRing(lblRing3, _ring3);
                }
            }
        }

        private void BulbOn(string encryptedLetter)
        {
            Label[] bulbs = new Label[25];
            bulbs[0] = lblQBulb;
            bulbs[1] = lblWBulb;
            bulbs[2] = lblEBulb;
            bulbs[3] = lblRBulb;
            bulbs[4] = lblTBulb;
            bulbs[5] = lblYBulb;
            bulbs[6] = lblUBulb;
            bulbs[7] = lblIBulb;
            bulbs[8] = lblOBulb;
            bulbs[9] = lblPBulb;

            bulbs[10] = lblABulb;
            bulbs[11] = lblSBulb;
            bulbs[12] = lblDBulb;
            bulbs[13] = lblFBulb;
            bulbs[14] = lblGBulb;
            bulbs[15] = lblHBulb;
            bulbs[16] = lblJBulb;
            bulbs[17] = lblKBulb;
            bulbs[18] = lblLBulb;

            bulbs[19] = lblZBulb;
            bulbs[20] = lblXBulb;
            bulbs[21] = lblCBulb;
            bulbs[22] = lblVBulb;
            bulbs[23] = lblNBulb;
            bulbs[24] = lblMBulb;

            
            int temp = -1;
            for (int x = 0; x < 25; x++)
            {
                if (bulbs[x].Content.ToString() == encryptedLetter)
                {
                    bulbs[x].Background = new SolidColorBrush(Colors.Black);
                    temp = x;
                }
            }
        }
        private void btnQKey_Click(object sender, RoutedEventArgs e)
        {
            Input("Q");
        }

        private void btnWKey_Click(object sender, RoutedEventArgs e)
        {
            Input("W");
        }
        private void btnEKey_Click(object sender, RoutedEventArgs e)
        {
            Input("E");
        }

        private void btnRKey_Click(object sender, RoutedEventArgs e)
        {
            Input("R");
        }
        private void btnTKey_Click(object sender, RoutedEventArgs e)
        {
            Input("T");
        }
        private void btnYKey_Click(object sender, RoutedEventArgs e)
        {
            Input("Y");
        }
        private void btnUKey_Click(object sender, RoutedEventArgs e)
        {
            Input("U");
        }
        private void btnIKey_Click(object sender, RoutedEventArgs e)
        {
            Input("I");
        }
        private void btnOKey_Click(object sender, RoutedEventArgs e)
        {
            Input("O");
        }
        private void btnPKey_Click(object sender, RoutedEventArgs e)
        {
            Input("P");
        }
        private void btnAKey_Click(object sender, RoutedEventArgs e)
        {
            Input("A");
        }
        private void btnSKey_Click(object sender, RoutedEventArgs e)
        {
            Input("S");
        }
        private void btnDKey_Click(object sender, RoutedEventArgs e)
        {
            Input("D");
        }
        private void btnFKey_Click(object sender, RoutedEventArgs e)
        {
            Input("F");
        }
        private void btnGKey_Click(object sender, RoutedEventArgs e)
        {
            Input("G");
        }
        private void btnHKey_Click(object sender, RoutedEventArgs e)
        {
            Input("H");
        }
        private void btnJKey_Click(object sender, RoutedEventArgs e)
        {
            Input("J");
        }
        private void btnKKey_Click(object sender, RoutedEventArgs e)
        {
            Input("K");
        }
        private void btnLKey_Click(object sender, RoutedEventArgs e)
        {
            Input("L");
        }
        private void btnZKey_Click(object sender, RoutedEventArgs e)
        {
            Input("Z");
        }
        private void btnXKey_Click(object sender, RoutedEventArgs e)
        {
            Input("X");
        }
        private void btnCKey_Click(object sender, RoutedEventArgs e)
        {
            Input("C");
        }
        private void btnVKey_Click(object sender, RoutedEventArgs e)
        {
            Input("V");
        }
        private void btnBKey_Click(object sender, RoutedEventArgs e)
        {
            Input("B");
        }
        private void btnNKey_Click(object sender, RoutedEventArgs e)
        {
            Input("N");
        }
        private void btnMKey_Click(object sender, RoutedEventArgs e)
        {
            Input("M");
        }

        private void TurnOff_Click(object sender, RoutedEventArgs e)
        {
            Label[] bulbs = new Label[25];
            bulbs[0] = lblQBulb;
            bulbs[1] = lblWBulb;
            bulbs[2] = lblEBulb;
            bulbs[3] = lblRBulb;
            bulbs[4] = lblTBulb;
            bulbs[5] = lblYBulb;
            bulbs[6] = lblUBulb;
            bulbs[7] = lblIBulb;
            bulbs[8] = lblOBulb;
            bulbs[9] = lblPBulb;

            bulbs[10] = lblABulb;
            bulbs[11] = lblSBulb;
            bulbs[12] = lblDBulb;
            bulbs[13] = lblFBulb;
            bulbs[14] = lblGBulb;
            bulbs[15] = lblHBulb;
            bulbs[16] = lblJBulb;
            bulbs[17] = lblKBulb;
            bulbs[18] = lblLBulb;

            bulbs[19] = lblZBulb;
            bulbs[20] = lblXBulb;
            bulbs[21] = lblCBulb;
            bulbs[22] = lblVBulb;
            bulbs[23] = lblNBulb;
            bulbs[24] = lblMBulb;

            for (int x = 0; x < 25; x++)
            {
                bulbs[x].Background = new SolidColorBrush(Colors.LightBlue);
            }
        }
    }
}

