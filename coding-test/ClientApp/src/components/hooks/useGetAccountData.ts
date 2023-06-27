import * as React from 'react';

export interface Errors {
    [index: string]: any;
}

export interface AccountDetails {
    clientId: number;
    firstName: string;
    lastName: number;
}

export interface BankAccount {
    accountNumber: number;
    accountType: number;
    transactions: BankTransaction[];
}

export interface BankTransaction {
    transactionId: number;
    accountNumber: number;
    amount: number;
    addedOn: Date;
}

export const useGetAccountData = (): { accountDetails: AccountDetails, accounts: BankAccount[], fetchAccounts: () => Promise<void>, isLoading: boolean, error: Errors } => {
    const [isLoading, setIsloading] = React.useState(true);
    const [error, setError] = React.useState<Errors>({});
    const [accountDetails, setAccountDetails] = React.useState<AccountDetails>();
    const [accounts, setAccounts] = React.useState<BankAccount[]>([]);

    const fetchAccounts = async () => {

        try {

            // intended to only fetch ClientId 1 accounts  (Me) 
            const response = await fetch("bankaccount/accounts/1");

            setAccounts(await response.json() as BankAccount[]);

        } catch (err) {
            setError(err);
        }
    }

    const fetchClientData = async () => {

        try {
            // intended to only fetch ClientId 1 Data  (Me) 
            const response = await fetch("bankaccount/1");

            setAccountDetails(await response.json() as AccountDetails);

        } catch (err) {
            setError(err);
        }
    }

    React.useEffect(() => {
        (async () => {
            setIsloading(true);
            await fetchClientData();
            await fetchAccounts();
            setIsloading(false);
        })();
    }, []);

    return { accountDetails, accounts, fetchAccounts, isLoading, error };
}