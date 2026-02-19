import { useState } from 'react';
import { updatePerson } from '../api/personApi';

const EditForm = ({ person, onCancel, onSaved }) => {
  const [errors, setErrors] = useState(null);
  const [form, setForm] = useState({
    name: person.name,
    birthDate: person.birthDate.split('T')[0],
    relationshipType: person.relationshipType,
    photo: person.photoUrl.split("/").pop()
  });

  const handleChange = e => {
    const { name, value } = e.target;
    setForm(prev => ({ ...prev, [name]: value }));
  };

  const handleSubmit = async e => {
    e.preventDefault();
    try {
      const formData = new FormData();
      formData.append('name', form.name);
      formData.append('birthDate', form.birthDate);
      formData.append('relationshipType', form.relationshipType);
      if (form.photo) {
        formData.append('photo', form.photo); // добавляем файл, если выбран
      }

      await updatePerson(person.guid, formData);
      onSaved();   // обновляем список
      onCancel();  // закрываем форму
    } catch (err) {
      setErrors(err.messages || [err.message]);
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

      <label className="file-upload">
        Upload photo
        <input
          type="file"
          name="photo"
          accept="image/*"
          onChange={(e) =>
            setForm(prev => ({ ...prev, photo: e.target.files[0] }))
          }
        />
      </label>

      {form.photo && (
        <span className="file-name">{form.photo instanceof File ? form.photo.name : form.photo}</span>
      )}

      <div className="form-actions">
        <button className="button" type="submit">Save</button>
        <button className="button" type="button" onClick={onCancel}>
          Cancel
        </button>
      </div>
      {errors && (
        <div className="form-error">
          {errors.map((e, i) => <p key={i}>{e}</p>)}
        </div>
      )}
    </form>
  );
};

export default EditForm;