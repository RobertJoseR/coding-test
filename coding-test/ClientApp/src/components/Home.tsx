import * as React from 'react';
import AccordionItem from './AccordionItem';
import CreateAccount from './CreateAccount';
import { BankTransaction, useGetAccountData } from './hooks/useGetAccountData';
import { ButtonGroup, Button, Spinner } from 'reactstrap';
import MakeTransaction from './MakeTransaction';
import RemoveAccount from './RemoveAccount';

const Home: React.FC = () => {
    const [openCreateModal, setOpenCreateModal] = React.useState(false);
    const [transactionSelected, setTransactionSelected] = React.useState<"" | "DEPOSIT" | "WITHDRAWAL">("");
    const [openRemoveModal, setOpenRemoveModal] = React.useState(false);

    const [currentAccount, setCurrentAccount] = React.useState(0);
    const { accountDetails, accounts, fetchAccounts, isLoading, error } = useGetAccountData();

    const onAddAccount = () => {
        // Open Modal
        setOpenCreateModal(true);
    };

    const selectAccount = (id: number) => {
        setCurrentAccount(id);
    };

    if (isLoading) {
        return <div className="d-flex align-items-center justify-content-center">
            <Spinner
                color="primary"
                type="grow"
            />
        </div>;
    }

    return (
        <React.Fragment>
            <div>
                <h1>Hello, {`${accountDetails.firstName}, ${accountDetails.lastName}`}!</h1>
                {openCreateModal && <CreateAccount
                    accountDetails={accountDetails}
                    handleToggle={async () => {
                        await fetchAccounts();
                        setOpenCreateModal(false);
                    }}
                />
                }

                {transactionSelected !== "" && <MakeTransaction
                    transactionType={transactionSelected}
                    accountNumber={currentAccount}
                    closeModal={async () => {
                        await fetchAccounts();
                        setTransactionSelected("");
                    }}
                />
                }

                {openRemoveModal && <RemoveAccount
                    accountNumber={currentAccount}
                    closeModal={async () => {
                        await fetchAccounts();
                        setOpenRemoveModal(false);
                    }}
                />
                }

                <div className="pb-2">
                    <button
                        type="button"
                        className="btn btn-primary"
                        onClick={onAddAccount}
                    >
                        Create Account +
                    </button>
                </div>

                <div className="accordion" id="accordion">
                    {accounts.map((acc) => <AccordionItem
                        id={acc.accountNumber}
                        showItem={currentAccount === acc.accountNumber}
                        title={`Account Type: ${acc.accountType} - # ${acc.accountNumber}`}
                        onClick={selectAccount}
                        key={acc.accountNumber}
                    >
                        <div>

                            <h5> Account Information </h5>

                            <br />

                            <table className="table table-striped">
                                <thead>
                                    <tr>
                                        <th scope="col">#</th>
                                        <th scope="col">Deposit</th>
                                        <th scope="col">WithDrawal</th>
                                        <th scope="col">Date</th>
                                    </tr>
                                </thead>
                                <tbody>
                                    {acc.transactions.map((t: BankTransaction, index:number) => {

                                        return <tr key={t.transactionId}>
                                            <th scope="row">{index +1 }</th>
                                            <td>{t.amount > 0 ? t.amount : ''}</td>
                                            <td>{t.amount < 0 ? t.amount : ''}</td>
                                            <td>{t.addedOn.toString()}</td>
                                        </tr>;
                                    })}
                                </tbody>
                            </table>

                            <div className="p-4">
                                Account Balance: {acc.transactions.map(i => i.amount).reduce((a, b) => a + b, 0)}
                            </div>

                            <div className="pt-4 d-flex flex-row-reverse">
                                <ButtonGroup>
                                    <Button color="primary" onClick={() => setTransactionSelected("DEPOSIT")}>
                                        Make Deposit
                                    </Button>
                                    <Button color="secondary" onClick={() => setTransactionSelected("WITHDRAWAL")}>
                                        Widrawal
                                    </Button>
                                    <Button color="danger" onClick={() => setOpenRemoveModal(true)} >
                                        Delete Account
                                    </Button>
                                </ButtonGroup>
                            </div>
                        </div>
                    </AccordionItem>
                    )}

                </div>

            </div>
        </React.Fragment >
    );
}

export default Home;
