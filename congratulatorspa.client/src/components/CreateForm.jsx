import { useState } from 'react';
import { createPerson } from '../api/personApi';

const CreateForm = ({ onCreated, onCancel }) => {
  const [error, setError] = useState(null);
  const [form, setForm] = useState({
    name: '',
    birthDate: '',
    relationshipType: ''
  });
  const handleChange = (e) => {
    const { name, value } = e.target;
    setForm(prev => ({
      ...prev,
      [name]: value
    }));
  };

  const handleSubmit = async (e) => {
    e.preventDefault();

    try {
      await createPerson(form);
      onCreated();   // обновляем список
      onCancel();    // закрываем форму
    } catch (err) {
      setError(err.message);
    }
  };
return (
    <form className="create-form" onSubmit={handleSubmit}>
      <input
        type="text"
        name="name"
        placeholder="Name"
        value={form.name}
        onChange={handleChange}
        required
      />

      <input
        type="date"
        name="birthDate"
        value={form.birthDate}
        onChange={handleChange}
        required
      />

      <select
        name="relationshipType"
        value={form.relationshipType}
        onChange={handleChange}
        required
      >
        <option value="">Select relationship</option>
        <option value="Unknown">Unknown</option>
        <option value="Known">Known</option>
        <option value="Friend">Friend</option>
        <option value="Relative">Relative</option>
        <option value="Coworker">Coworker</option>
      </select>

      <div className="form-actions">
        <button type="submit" className="button">
          Create
        </button>
        <button type="button" className="button" onClick={onCancel}>
          Cancel
        </button>
      </div>
      {error && (
        <div className="form-error">
          {error}
        </div>
      )}
    </form>
  );
};

export default CreateForm;