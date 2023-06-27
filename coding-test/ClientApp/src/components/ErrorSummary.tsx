import * as React from 'react';
import { Errors } from './hooks/useGetAccountData';

interface Props {
    err: Errors
}
const ErrorSummary: React.FC<Props> = ({ err}) => {
    if (Object.keys(err).length === 0) { return null; }

    return <div className="text-danger text-center">
        {err.message}
    </div>
}

export default ErrorSummary;