import * as React from 'react';
import { Errors } from './useGetAccountData';

export const useBankOperations = (): {
    createNewAccount: (data: FormData) => Promise<boolean>,
    makeDeposit: (data: FormData) => Promise<boolean>,
    withdrawal: (data: FormData) => Promise<boolean>,
    removeAccount: (accountNumber: number) => void
    isLoading: boolean,
    error: Errors
} => {
    const [isLoading, setIsloading] = React.useState(false);
    const [error, setError] = React.useState<Errors>({});

    const createNewAccount = async (data: FormData) => {

        try {

            setIsloading(true);
            setError({});

            const response = await fetch('bankaccount', {
                method: "POST",
                body: data
            });

            if (response.ok) {
                return true;
            } else {
                throw new Error(
                    JSON.parse(await response.text()).message
                );
            }
        }
        catch (e) {
            setError({message: e.message});
            console.log("ERROR", e)
            return false;
        }
        finally {
            setIsloading(false)
        };
    };

    const makeTransaction = async (url: string, data: FormData) => {
        try {

            setIsloading(true);
            setError({ });

            const response = await fetch(url, {
                method: "POST",
                body: data
            });

            if (response.ok) {
                return true;
            } else {
                throw new Error(
                    JSON.parse(await response.text()).message
                );
            }
        }
        catch (e) {
            setError({ message: e.message });
            console.log("ERROR", e)
            return false;
        }
        finally {
            setIsloading(false)
        };
    }

    const makeDeposit = async (data: FormData) => {

       return await makeTransaction('bankoperations/deposit', data);

    };


    const withdrawal = async (data: FormData) => {

        return await makeTransaction('bankoperations/withdrawal', data);

    };

    const removeAccount = async (accountNumber: number) => {

        try {

            setIsloading(true);
            setError({});

            const response = await fetch(`bankaccount/account/${accountNumber}`, {
                method: "DELETE",
            });

            return response.json();
        } catch (err) {
            console.log("ERROR", err)
            setError(err);
        }
    };


    return { removeAccount, createNewAccount, makeDeposit, withdrawal, isLoading, error };
}