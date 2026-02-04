import { useState } from 'react';
import { updatePerson } from '../api/personApi';

const EditForm = ({ person, onCancel, onSaved }) => {
  const [form, setForm] = useState({
    name: person.name,
    birthDate: person.birthDate.split('T')[0],
    relationshipType: person.relationshipType
  });

  const handleChange = e => {
    const { name, value } = e.target;
    setForm(prev => ({ ...prev, [name]: value }));
  };

  const handleSubmit = async e => {
    e.preventDefault();
    try {
        await updatePerson(person.guid, form);
        onSaved();   // обновляем список
        onCancel();    // закрываем форму
    } catch (err) {
        console.error(err);
    }
  };

  return (
    <form className="edit-form" onSubmit={handleSubmit}>
      <input
        name="name"
        value={form.name}
        onChange={handleChange}
      />

      <input
        type="date"
        name="birthDate"
        value={form.birthDate}
        onChange={handleChange}
      />

      <select
        name="relationshipType"
        value={form.relationshipType}
        onChange={handleChange}
      >
        <option value="Unknown">Unknown</option>
        <option value="Known">Known</option>
        <option value="Friend">Friend</option>
        <option value="Relative">Relative</option>
        <option value="Coworker">Coworker</option>
      </select>

      <div className="form-actions">
        <button className="button" type="submit">Save</button>
        <button className="button" type="button" onClick={onCancel}>
          Cancel
        </button>
      </div>
    </form>
  );
};

export default EditForm;