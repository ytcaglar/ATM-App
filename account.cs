namespace ATM;
public class account
{
    private decimal _balance = 0;
    private decimal _creditCard = 0;


    public string Name  { get; set; }
    public string SurName  { get; set; }
    public string AccountId  { get; set; }
    public string Password  { get; set; }
    public decimal Balance  
    { 
        get
        {
            return _balance;
        } 
        set
        {
            _balance = value;
        } 
    }
    public decimal CreditCard  
    { 
        get
        {
            return _creditCard;
        } 
        set
        {
            _creditCard = value;
        } 
    }

    public void Deposit(decimal amount){
        _balance+=amount;
    }

    public void Withdraw(decimal amount){
        _balance-=amount;
    }

    public void CreditCardPayment(decimal amount){
        _creditCard-=amount;
    }

    public void CreditCardWithdraw(decimal amount){
        _creditCard-=amount;
    }



    
}
