import { deletePerson } from '../api/personApi';

const DeleteConfirm = ({ guid, name, onDeleted, onCancel }) => {
  const handleDelete = async () => {
    try {
      await deletePerson(guid);
      onDeleted(); // обновить список
      onCancel();   
    } catch (err) {
      console.error(err);
    }
  };

  return (
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
  );
};

export default DeleteConfirm;