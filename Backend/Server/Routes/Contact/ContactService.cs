using Microsoft.EntityFrameworkCore;
using Backend.Server.Config;

namespace Backend.Server.Routes.Contact;

public class ContactService(Database Context)
{
    public async Task<IEnumerable<ContactEntity>> GetAll()
    {
        var contacts = await Context.Contacts
            .Where(c => c.DeletedAt == DateTime.MinValue)
            .Select(c => new ContactEntity
            {
                Id = c.Id,
                Name = c.Name,
                Phone = c.Phone,
                Email = c.Email,
                DeletedAt = c.DeletedAt,
                CreatedAt = c.CreatedAt,
                UpdatedAt = c.UpdatedAt
            })
            .ToListAsync();

        return contacts;
    }

    public async Task<ContactEntity> GetById(int id)
    {
        var contact = await Context.Contacts
            .Where(c => c.DeletedAt == DateTime.MinValue)
            .Select(c => new ContactEntity
            {
                Id = c.Id,
                Name = c.Name,
                Phone = c.Phone,
                Email = c.Email,
                DeletedAt = c.DeletedAt,
                CreatedAt = c.CreatedAt,
                UpdatedAt = c.UpdatedAt
            })
            .FirstOrDefaultAsync() ?? throw new KeyNotFoundException($"Contato com ID {id} não encontrado");

        return contact;
    }

    public async Task<ContactEntity> Create(CreateContactDto createContactDto)
    {
        var contact = new ContactEntity
        {
            Name = createContactDto.Name,
            Phone = createContactDto.Phone,
            Email = createContactDto.Email,
            DeletedAt = DateTime.MinValue,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        Context.Contacts.Add(contact);
        await Context.SaveChangesAsync();

        return new ContactEntity
        {
            Id = contact.Id,
            Name = contact.Name,
            Phone = contact.Phone,
            Email = contact.Email,
            DeletedAt = contact.DeletedAt,
            CreatedAt = contact.CreatedAt,
            UpdatedAt = contact.UpdatedAt
        };
    }

    public async Task<ContactEntity> Update(int id, UpdateContactDto updateContactDto)
    {
        var contact = await Context.Contacts.FirstOrDefaultAsync(c => c.Id == id && c.DeletedAt == DateTime.MinValue) ??
            throw new KeyNotFoundException($"Contato com ID {id} não encontrado");

        if (!string.IsNullOrEmpty(updateContactDto.Name)) contact.Name = updateContactDto.Name;
        if (!string.IsNullOrEmpty(updateContactDto.Phone)) contact.Phone = updateContactDto.Phone;
        if (!string.IsNullOrEmpty(updateContactDto.Email)) contact.Email = updateContactDto.Email;

        contact.UpdatedAt = DateTime.UtcNow;

        await Context.SaveChangesAsync();

        return new ContactEntity
        {
            Id = contact.Id,
            Name = contact.Name,
            Phone = contact.Phone,
            Email = contact.Email,
            DeletedAt = contact.DeletedAt,
            CreatedAt = contact.CreatedAt,
            UpdatedAt = contact.UpdatedAt
        };
    }

    public async Task Delete(int id)
    {
        var contact = await Context.Contacts.FirstOrDefaultAsync(c => c.Id == id && c.DeletedAt == DateTime.MinValue)
        ?? throw new KeyNotFoundException($"Contato com ID {id} não encontrado");

        contact.DeletedAt = DateTime.UtcNow;
        contact.UpdatedAt = DateTime.UtcNow;

        await Context.SaveChangesAsync();
    }
}