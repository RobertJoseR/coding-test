import * as React from 'react';
import {
    Form,
    FormGroup,
    Label,
    Input,
    Button,
    Modal,
    ModalHeader,
    ModalBody,
    ModalFooter,
    Col,
    Spinner
} from 'reactstrap';
import ErrorSummary from './ErrorSummary';
import { useBankOperations } from './hooks/useBankOperations';

interface Props {
    accountNumber: number;
    closeModal: () => void;
    transactionType: "DEPOSIT" | "WITHDRAWAL" | "";
}


const MakeTransaction: React.FC<Props> = ({ accountNumber, closeModal, transactionType }) => {

    const { withdrawal, makeDeposit, error, isLoading } = useBankOperations();
    const [modal, setModal] = React.useState(true);

    const toggle = () => {
        setModal(!modal);
        closeModal();
    }

    const handleSubmit = async (e) => {
        e.preventDefault();

        const formElement = document.querySelector("form");

        const data = new FormData(formElement);

        const result = transactionType === 'DEPOSIT'
            ? await makeDeposit(data)
            : await withdrawal(data)

        if (result) {
            closeModal();
        }
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
        <div>
            <Modal isOpen={modal} toggle={toggle}>
                <ModalHeader toggle={toggle}>Create New Account</ModalHeader>
                <ModalBody>
                    <Form onSubmit={handleSubmit}>
                        <div style={{ display: "none" }}>
                            <Input
                                id="accountNumber"
                                name="accountNumber"
                                type="number"
                                value={accountNumber}
                            />
                        </div>

                        <Col md={12}>
                            <FormGroup>
                                <Label for="Amount">
                                    {`AMOUNT TO ${transactionType}`}
                                </Label>
                                <Input
                                    id="amount"
                                    name="amount"
                                    type="number"
                                />
                            </FormGroup>
                        </Col>

                        <Button type="submit">
                            {transactionType}
                        </Button>

                    </Form>
                    <ErrorSummary err={error} />
                </ModalBody>
                <ModalFooter>
                    <Button color="secondary" onClick={toggle}>
                        Cancel
                    </Button>
                </ModalFooter>
            </Modal>
        </div>
    );
}

export default MakeTransaction;

