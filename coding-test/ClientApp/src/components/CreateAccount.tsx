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
import { AccountDetails } from './hooks/useGetAccountData';

interface Props {
    accountDetails: AccountDetails;
    handleToggle: () => void;
}

const CreateAccount: React.FC<Props> = ({ handleToggle, accountDetails }) => {
    const { createNewAccount, error, isLoading } = useBankOperations();
    const [modal, setModal] = React.useState(true);

    const toggle = () => {
        setModal(!modal);
        handleToggle();
    }

    const handleSubmit = async (e) => {
        e.preventDefault();

        const formElement = document.querySelector("form");

        const data = new FormData(formElement);

        if (await createNewAccount(data)) {
            toggle();
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
                                id="clientNumber"
                                name="clientNumber"
                                type="number"
                                value={accountDetails.clientId}
                            />
                        </div>
                        <Col md={12}>
                            <FormGroup>
                                <Label
                                    for="accountType"
                                >
                                    Email
                                </Label>
                                <Input
                                    id="accountType"
                                    name="accountType"
                                    className="mb-3"
                                    type="select"
                                >
                                    <option>
                                        Savings
                                    </option>
                                    <option>
                                        Checkings
                                    </option>
                                </Input>
                            </FormGroup>
                        </Col>

                        <Col md={12}>
                            <FormGroup>
                                <Label for="balance">
                                    Initial Deposit
                                </Label>
                                <Input
                                    id="balance"
                                    name="balance"
                                    placeholder="initial deposit amount"
                                    type="number"
                                />
                            </FormGroup>
                        </Col>
                        <div>
                            <Label className="danger" > {error.Error} </Label>
                        </div>
                        <Button type="submit">
                            Create
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

export default CreateAccount;
