import { useState, useEffect } from 'react'
import { getPeople } from '../api/personApi'

function getBirthdays(people, sortBy = 'name') {
  return [...people].sort((a, b) => {
    switch (sortBy.toLowerCase()) {
      case 'name':
        return a.name.localeCompare(b.name);
      case 'birthdate':
        return new Date(a.birthDate) - new Date(b.birthDate);
      case 'relationship':
        return a.relationshipType.localeCompare(b.relationshipType);
      default:
        return 0;
    }
  });
}

const AllBirthdays = () => {
    const [people, setPeople] = useState([]);
    const [sortBy, setSortBy] = useState('name');
  
     useEffect(() => {
      getPeople()
        .then(data => {
          const sorted = getBirthdays(data, sortBy);
          setPeople(sorted);
        })
        .catch(err => console.error(err));
    }, [sortBy]);

  return (
    <main className='container'>
      <h1>All Birthdays</h1>
      <div className='sort'>
        <label>
            Sort by:{' '}
            <select value={sortBy} onChange={e => setSortBy(e.target.value)}>
                <option value="name">Name</option>
                <option value="birthdate">Birthdate</option>
                <option value="relationship">Relationship</option>
            </select>
        </label> 
      </div>
      <table>
        <thead>
          <tr>
            <th>Name</th>
            <th>Birthdate</th>
            <th>Relationship</th>
          </tr>
        </thead>
        <tbody>
          {people.map(p => (
            <tr key={p.guid}>
              <td>{p.name}</td>
              <td>{new Date(p.birthDate).toLocaleDateString('ru-RU')}</td>
              <td>{p.relationshipType}</td>
            </tr>
          ))}
        </tbody>
      </table>
    </main>
  )
}

export default AllBirthdays