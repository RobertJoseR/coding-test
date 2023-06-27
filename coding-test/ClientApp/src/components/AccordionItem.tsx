import * as React from 'react';

interface Props {
    id: number,
    title: string,
    showItem: boolean,
    children: React.ReactNode,
    onClick: (id:number) => void;
}

const AccordionItem: React.FC<Props> = ({ id, showItem, children, title, onClick }) => {

    return <React.Fragment>
        <div className="accordion-item" onClick={() => { onClick(id) }}>
            <h2 className="accordion-header" id={title}>
                <button className={`accordion-button ${showItem ? '' : 'collapsed'}`} type="button" data-bs-toggle="collapse" data-bs-target="#collapseOne" aria-expanded={showItem} aria-controls="collapseOne">
                    <h4>{title}</h4>
                </button>
            </h2>
            <div id={`body${title}`} className={`accordion-collapse collapse ${showItem ? 'show' : ''}`} aria-labelledby="headingOne" data-bs-parent="#accordionExample">
                <div className="accordion-body" style={{backgroundColor: "lightgray"}}>
                    {children}
                </div>
            </div>
        </div>
    </React.Fragment >
}

export default AccordionItem;