namespace ATM;
public static class atmMethod
{
    public static List<string> log = new List<string>();
    public static List<account> accountList = new List<account>();
    public static account nowAccount = null;
    public static decimal bankBalance;
    public static bool exitControl = true;

    public static void MainMenu(){
        firstCreateFile();
        while(exitControl){
            if (nowAccount != null)
            {
                accountMenu();
            }else{
                firstMenu();
            }
        }
        SaveUserInfo();
        BankBalance();
        logDaily();
    }

    public static void firstMenu(){
        Back:
        System.Console.WriteLine(" - Welcome to the banking system -");
        System.Console.WriteLine("1 - Login\n2 - Register\n3 - End of Day Report\n0 - Exit");
        System.Console.Write("Please, Select One : ");
        if(Int32.TryParse(Console.ReadLine(), out int chose)){
            switch (chose)
            {
                case 1:
                    LogIn();
                    break;
                case 2:
                    createAccountMenu();
                    break;
                case 3:
                    ShowlogDaily();
                    break;
                case 0:
                    System.Console.WriteLine("You are exiting the banking system.");
                    exitControl = false;
                    break;
                default:
                    System.Console.WriteLine("You chose wrong. Please select an option from the menu again...");
                    goto Back;
            }
        }else{
            System.Console.WriteLine("You chose wrong. Please select an option from the menu again...");
            goto Back;
        }
    }

    public static void accountMenu(){
        Back:
        System.Console.WriteLine($"Welcome {nowAccount.Name} {nowAccount.SurName},");
        System.Console.WriteLine($"Your Balance : {nowAccount.Balance}");
        System.Console.WriteLine("1 - Deposit\n2 - Withdraw\n3 - Change Password\n4 - Logout\n5 - Show End of Day Report\n0 - Exit");
        System.Console.Write("Please, Select One : ");
        if(Int32.TryParse(Console.ReadLine(), out int chose)){
            switch (chose)
            {
                case 1:
                    Deposit();
                    break;
                case 2:
                    Withdraw();
                    break;
                case 3:
                    ChangePassword();
                    break;
                case 4:
                    Logout();
                    break;
                case 5:
                    ShowAccountlogDaily();
                    break;
                case 0:
                    Exit();
                    break;
                default:
                    System.Console.WriteLine("You chose wrong. Please select an option from the menu again...");
                    goto Back;
            }
        }else{
            System.Console.WriteLine("You chose wrong. Please select an option from the menu again...");
            goto Back;
        }
    }

    public static void createAccountMenu(){
        System.Console.WriteLine("- Welcome Register System -\n");
        IdNum:
        System.Console.Write("Identification Number : ");
        string IdNum = Console.ReadLine().Trim();
        if(IdNumControl(IdNum))
        {   
            account user =  isThereID(IdNum);
            if( user != null){
                System.Console.WriteLine("Someone with the same ID number is registered in the system.");
                Log($"Process : Registration | A registered person ({user.Name} {user.SurName}) with ID : {IdNum} wanted to register again. ");
                goto End;
            }
            password:
            System.Console.Write("Password : ");
            string password = Console.ReadLine().Trim();
            if(password.Length > 15 && password.Length < 5){
                System.Console.WriteLine("Please, enter a valid ID number.");
                goto password;
            }
            Name:
            System.Console.Write("Name : ");
            string name = Console.ReadLine().Trim();
            if(name.Length > 15 && name.Length < 1){
                System.Console.WriteLine("Please, enter a valid name.");
                goto Name;
            }
            SurName:
            System.Console.Write("Surname : ");
            string surName = Console.ReadLine().Trim();
            if(surName.Length > 15 && surName.Length < 1){
                System.Console.WriteLine("Please, enter a valid surname.");
                goto SurName;
            }
            System.Console.WriteLine("User registered.");
            accountList.Add(new account{Name = name, SurName = surName, AccountId = IdNum, Password=password});
            Log($"Process : Registration | {name} {surName} registered.");
        }else
        {
            System.Console.WriteLine("Please, enter a valid ID number.");
            goto IdNum;
        }
        End:
            System.Console.WriteLine();
    }

    public static void LogIn()
    {
        logDaily();
        System.Console.WriteLine("- Welcome Login System -\n");
        IdNum:
        System.Console.Write("Identification Number : ");
        string IdNum = Console.ReadLine().Trim();
        if(IdNumControl(IdNum))
        {
            account user = isThereID(IdNum);
            if(user == null){
                System.Console.WriteLine("Unregistered user.");
                Log($"Process : Login | Unregistered person ID : {IdNum}");
            }else{
                password:
                System.Console.Write("Password : ");
                string password = Console.ReadLine().Trim();
                if(password.Length > 15 && password.Length < 5){
                    System.Console.WriteLine("Please, enter a valid ID number.");
                    goto password;
                }else if(user.Password==password){
                    nowAccount = user;
                    Log($"Process : Login | {nowAccount.Name} {nowAccount.SurName} entered the banking system.");

                }else{
                    Log($"Process : Login | Wrong Password... person ID : {IdNum} and wrong password : {password}");
                    System.Console.WriteLine("Wrong password...");
                    goto password;
                }
            }
            
        }else{
            System.Console.WriteLine("Please, enter a valid ID number.");
            goto IdNum;
        }
    }

    private static void Logout(){
        System.Console.WriteLine($"See you, {nowAccount.Name} {nowAccount.SurName}..");
        Log($"Process : Logout | {nowAccount.Name} {nowAccount.SurName} logged out of the bank system.");
        AccountlogDaily();
        nowAccount = null;
        System.Console.WriteLine("The bank account has been logged out.");
    }

    private static void Exit()
    {
        System.Console.WriteLine("You are exiting the banking system.");
        exitControl = false;
    }
    
    private static void Withdraw(){
        Back:
        System.Console.WriteLine("- Money Withdrawal -");
        System.Console.Write("Amount : ");
        if (Int32.TryParse(Console.ReadLine(), out int amount))
        {
            if (nowAccount.Balance>=amount && amount>0)
            {
                nowAccount.Withdraw(amount);
                System.Console.WriteLine($"Amount Withdrawn : {amount} ₺ - Balance : {nowAccount.Balance} ₺");
                Log($"Process : Withdraw | {nowAccount.Name} {nowAccount.SurName} | Amount Withdrawn : {amount} ₺ - Balance : {nowAccount.Balance} ₺");
            }else
            {
                System.Console.WriteLine("Your account balance is insufficient.");
                Log($"Process : Withdraw | {nowAccount.Name} {nowAccount.SurName} | Balance is insufficient. Amount Withdrawn : {amount} ₺ - Balance : {nowAccount.Balance} ₺");
            }
        }else{
            System.Console.WriteLine("Please, enter a valid amount.");
            goto Back;
        }
    }

    private static void Deposit(){
        Back:
        System.Console.WriteLine("- Money Deposit -");
        System.Console.Write("Amount : ");
        if (Int32.TryParse(Console.ReadLine(), out int amount))
        {
            if (amount>0)
            {
                nowAccount.Deposit(amount);
                System.Console.WriteLine($"Amount Deposit : {amount} ₺ - Balance : {nowAccount.Balance} ₺");
                Log($"Process : Deposit | {nowAccount.Name} {nowAccount.SurName} | Amount Deposit : {amount} ₺ - Balance : {nowAccount.Balance} ₺");
            }else
            {
                System.Console.WriteLine("An incorrect amount was entered.");
                Log($"Process : Deposit | {nowAccount.Name} {nowAccount.SurName} | An incorrect amount was entered.");
            }
        }else{
            System.Console.WriteLine("Please, enter a valid amount.");
            goto Back;
        }
    }

    private static void ChangePassword()
    {
        ChangePassword:
        System.Console.WriteLine("- Change Password -");
        password:
            System.Console.Write("Please, enter your old Password : ");
            string password = Console.ReadLine().Trim();
            if(password.Length > 15 && password.Length < 5){
                System.Console.WriteLine("Please, enter a valid ID number.");
                goto password;
            }
            else if(password == nowAccount.Password)
            {
                NewPassword:
                System.Console.Write("Please, enter your new Password : ");
                string newpassword = Console.ReadLine().Trim();
                if(newpassword.Length > 15 && newpassword.Length < 5){
                    System.Console.WriteLine("Please, enter a valid ID number.");
                    goto NewPassword;
                }
                else
                {
                    nowAccount.Password = newpassword;
                    Log($"Process : Change Password | Person ID : {nowAccount.AccountId} | {nowAccount.Name} {nowAccount.SurName}'s Password Has Been Updated.");
                    System.Console.WriteLine("Your password has been updated.");
                }
            }else{
                System.Console.WriteLine("Wrong password...");
                Log($"Process : Change Password | Wrong Password... person ID : {nowAccount.AccountId} | User = {nowAccount.Name} {nowAccount.SurName} and wrong password : {password}");
                Back:
                System.Console.Write("- Menu -\n1 - Change Password Again\n2 - Back\n0 - Exit\nChose one : ");
                string chose = Console.ReadLine();
                if (chose == "1")
                {
                    goto ChangePassword;
                    
                }
                else if (chose == "2")
                {
                    
                }
                else if (chose == "0")
                {
                    Exit();
                }else
                {
                    System.Console.WriteLine("You chose wrong. Please select an option from the menu again...");
                    goto Back;
                }
            }
    }

    // For log daily
    private static void Log(string message){
        string time  = logTime()[1];
        log.Add($"Time : {time} | {message}");
    }

    private static void logDaily(){
        string date  = logTime()[0];
        FileStream fs = new FileStream($"Save/{date}.txt",FileMode.Append,FileAccess.Write,FileShare.Write);
        StreamWriter sw = new StreamWriter(fs);
        foreach (var item in log)
        {
            sw.WriteLine($"{item}");
        }
        sw.Close();
        log = new List<string>();
    }

    private static void AccountlogDaily(){
        string date  = logTime()[0];
        FileStream fs = new FileStream($"Save/{nowAccount.AccountId}-{date}.txt",FileMode.Append,FileAccess.Write,FileShare.Write);
        StreamWriter sw = new StreamWriter(fs);
        foreach (var item in log)
        {
            sw.WriteLine($"{item}");
        }
        sw.Close();
        logDaily();
    }

    private static void ShowlogDaily(){
        logDaily();
        string date  = logTime()[0];
        
        StreamReader sr = new StreamReader($"Save/{date}.txt");
        string line = sr.ReadToEnd();
        sr.Close();

        System.Console.WriteLine($"----------------------------- End Of Day ({date}) Report -----------------------------");
        System.Console.WriteLine(line);
    }

    private static void ShowAccountlogDaily(){
        AccountlogDaily();
        string date  = logTime()[0];
        
        StreamReader sr = new StreamReader($"Save/{nowAccount.AccountId}-{date}.txt");
        string line = sr.ReadToEnd();
        sr.Close();


        System.Console.WriteLine($"----------------------- {nowAccount.Name} {nowAccount.SurName} - End Of Day ({date}) Report -----------------------");
        System.Console.WriteLine(line);
    }

    private static string[] logTime(){
        
        string dateTime = DateTime.Now.ToString();
        return dateTime.Split(' ');
    }

    // User Control Method

    private static bool IdNumControl(string num){

        if(num.Length != 11){
            return false;
        }
        foreach (var item in num)
        {
            if (!Char.IsNumber(item)) {
                return false;
            }
        }
        return true;
    }
    
    public static account isThereID(string Id){
        if(accountList.Count() != 0){
            foreach (var item in accountList)
            {
                if(item.AccountId == Id){
                    return item;
                }
            }
        }
        return null;
    }

    // User Registration Method, Local Register and User Information Update Methods

    private static void addAccount(string name, string surName, string ID, string password){
        accountList.Add(new account{Name = name, SurName = surName, AccountId = ID, Password = password});
    }

     public static void firstCreateFile(){
        Directory.CreateDirectory("Save");
        if (!File.Exists("Save/user.txt"))
        {
            FileStream fs = new FileStream($"Save/user.txt",FileMode.Create);
            fs.Close();
            
        }else{
            TakeUserInfo();
        }
    }

    private static void SaveUserInfo(){
        DeleteUserInfo();
        FileStream fs = new FileStream("Save/user.txt",FileMode.Append,FileAccess.Write,FileShare.Write);
        StreamWriter sw = new StreamWriter(fs);
        foreach (var item in accountList)
        {
            sw.WriteLine($"{item.Name},{item.SurName},{item.AccountId},{item.Password},{item.Balance},{item.CreditCard}");

        }
        sw.Close();
    }

    private static void TakeUserInfo(){
        StreamReader sr = new StreamReader("Save/user.txt");
        string line = sr.ReadToEnd();
        if (line != "")
        {
            string[] lines = line.Split("\n");
            foreach (var item in lines)
            {
                if(item != ""){
                    string[] items = item.Split(',');
                    accountList.Add(new account{AccountId =  items[2], Name = items[0] , SurName = items[1], Password=items[3], Balance = Int32.Parse(items[4]), CreditCard  = Int32.Parse(items[5]) });
                }

            }
        }
        sr.Close();
    }

    private static void DeleteUserInfo(){
        FileStream fs = new FileStream("Save/user.txt",FileMode.Create,FileAccess.Write,FileShare.Write);
        StreamWriter sw = new StreamWriter(fs);
        sw.Close();
    }

    // Bank Method

    private static void BankBalance(){
        decimal total = 0;
        string date = logTime()[0];
        string time = logTime()[1];
        FileStream fs = new FileStream($"Save/bank.txt",FileMode.Append, FileAccess.Write,FileShare.Write);
        StreamWriter sw = new StreamWriter(fs);
        if (accountList.Count != 0)
        {
            
            foreach (var item in accountList)
            {
                total += item.Balance;
            }
        }
        sw.WriteLine($"Date : {date} | Time : {time} | Bank Balance : {total}");
        sw.Close();
    }

}
