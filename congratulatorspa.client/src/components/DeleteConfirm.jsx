import { useState } from 'react'
import { deletePerson } from '../api/personApi';

const DeleteConfirm = ({ guid, name, onDeleted, onCancel }) => {
  const [error, setError] = useState(null);
  const handleDelete = async () => {
    try {
      await deletePerson(guid);
      onDeleted(); // обновить список
      onCancel();   
    } catch (err) {
      setError(err.message);
    }
  };

  return (
    <>
    <div className="delete-confirm">
      <p>
        Are you sure you want to delete <strong>{name}</strong>?
      </p>

      <div className="form-actions">
        <button className="button danger" onClick={handleDelete}>
          Delete
        </button>
        <button className="button" onClick={onCancel}>
          Cancel
        </button>
      </div>      
    </div>
    {error && (
        <div className="form-error">
          {error}
        </div>
    )}
    </>
  );  
};

export default DeleteConfirm;