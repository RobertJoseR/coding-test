import * as React from 'react';
import {
    Form,
    Button,
    Modal,
    ModalHeader,
    ModalBody,
    ButtonGroup,
} from 'reactstrap';
import { useBankOperations } from './hooks/useBankOperations';

interface Props {
    accountNumber: number;
    closeModal: () => void;
}

const RemoveAccount: React.FC<Props> = ({ accountNumber, closeModal }) => {

    const { removeAccount, error, isLoading } = useBankOperations();
    const [modal, setModal] = React.useState(true);

    const toggle = () => {
        setModal(!modal);
        closeModal();
    }

    const handleSubmit = async (e) => {
        e.preventDefault();

        await removeAccount(accountNumber);

        closeModal();
    };

    return (
        <div>
            <Modal isOpen={modal} toggle={toggle}>
                <ModalHeader toggle={toggle}>Remove Account ? </ModalHeader>
                <ModalBody>
                    <Form onSubmit={handleSubmit}>

                        Are you sure you want to remove account?
                        <div className="pt-4 d-flex flex-row-reverse">
                            <ButtonGroup>
                                <Button color="danger" type="submit">
                                    Ok
                                </Button>
                                <Button color="secondary" onClick={toggle}>
                                    Cancel
                                </Button>
                            </ButtonGroup>
                        </div>
                    </Form>
                </ModalBody>
            </Modal>
        </div>
    );
}

export default RemoveAccount;

